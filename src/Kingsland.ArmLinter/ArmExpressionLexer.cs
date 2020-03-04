using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kingsland.ArmLinter
{

    public static class ArmExpressionLexer
    {

        #region Methods

        public static Lexer Create()
        {
            return new Lexer()
                .AddRule('[', ArmExpressionLexer.ReadOpenBracketToken)
                .AddRule(']', ArmExpressionLexer.ReadCloseBracketToken)
                .AddRule('(', ArmExpressionLexer.ReadOpenParenToken)
                .AddRule(')', ArmExpressionLexer.ReadCloseParenToken)
                .AddRule(',', ArmExpressionLexer.ReadCommaToken)
                .AddRule('.', ArmExpressionLexer.ReadDotOperatorToken)
                .AddRule('\'', ArmExpressionLexer.ReadStringLiteralToken)
                .AddRule("[a-z|A-z|_]", ArmExpressionLexer.ReadIdentifierToken)
                .AddRule("[0-9|-]", ArmExpressionLexer.ReadIntegerToken)
                // whitespace
                .AddRule('\u0020', ArmExpressionLexer.ReadWhitespaceToken)
                .AddRule('\u000D', ArmExpressionLexer.ReadWhitespaceToken)
                .AddRule('\u000A', ArmExpressionLexer.ReadWhitespaceToken);
        }

        #endregion

        #region Lexer Methods

        private static (Token, SourceReader) ReadOpenBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('[');
            var extent = SourceExtent.From(sourceChar);
            return (new OpenBracketToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadCloseBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(']');
            var extent = SourceExtent.From(sourceChar);
            return (new CloseBracketToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadOpenParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('(');
            var extent = SourceExtent.From(sourceChar);
            return (new OpenParenToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadCloseParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(')');
            var extent = SourceExtent.From(sourceChar);
            return (new CloseParenToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadCommaToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(',');
            var extent = SourceExtent.From(sourceChar);
            return (new CommaToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadDotOperatorToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('.');
            var extent = SourceExtent.From(sourceChar);
            return (new DotOperatorToken(extent), nextReader);
        }

        private static (Token, SourceReader) ReadIdentifierToken(SourceReader reader)
        {
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            var nameChars = new StringBuilder();
            // firstIdentifierChar
            (sourceChar, thisReader) = thisReader.Read(ArmStringValidator.IsFirstIdentifierChar);
            sourceChars.Add(sourceChar);
            nameChars.Append(sourceChar.Value);
            // *( nextIdentifierChar )
            while (!thisReader.Eof() && thisReader.Peek(ArmStringValidator.IsNextIdentifierChar))
            {
                (sourceChar, thisReader) = thisReader.Read();
                sourceChars.Add(sourceChar);
                nameChars.Append(sourceChar.Value);
            }
            // return the result
            var extent = SourceExtent.From(sourceChars);
            var name = nameChars.ToString();
            return (new IdentifierToken(extent, name), thisReader);
        }

        private static (Token, SourceReader) ReadIntegerToken(SourceReader reader)
        {
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            var sign = 1;
            var value = 0;
            // read the sign
            switch (thisReader.Peek().Value)
            {
                case '-':
                    sign = -1;
                    (sourceChar, thisReader) = thisReader.Read('-');
                    sourceChars.Add(sourceChar);
                    break;
                case '+':
                    sign = +1;
                    (sourceChar, thisReader) = thisReader.Read('+');
                    sourceChars.Add(sourceChar);
                    break;
            }
            // digit
            (sourceChar, thisReader) = thisReader.Read(ArmStringValidator.IsDigit);
            sourceChars.Add(sourceChar);
            value = value * 10 + (sourceChar.Value - '0');
            // *( digit )
            while (!thisReader.Eof() && thisReader.Peek(ArmStringValidator.IsDigit))
            {
                (sourceChar, thisReader) = thisReader.Read();
                sourceChars.Add(sourceChar);
                value = value * 10 + (sourceChar.Value - '0');
            }
            // return the result
            var extent = SourceExtent.From(sourceChars);
            return (new IntegerToken(extent, sign * value), thisReader);
        }

        private static (Token, SourceReader) ReadStringLiteralToken(SourceReader reader)
        {
            const char SINGLEQUOTE = '\'';
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            var stringChars = new StringBuilder();
            // read the first single-quote
            (sourceChar, thisReader) = thisReader.Read(SINGLEQUOTE);
            sourceChars.Add(sourceChar);
            // read the remaining characters
            var isTerminated = false;
            while (!thisReader.Eof())
            {
                var peek = thisReader.Peek();
                sourceChars.Add(peek);
                if (peek.Value == SINGLEQUOTE)
                {
                    thisReader = thisReader.Next();
                    if (!thisReader.Eof() && (thisReader.Peek().Value == SINGLEQUOTE))
                    {
                        // an escaped single-quote (i.e. "''")
                        sourceChars.Add(peek);
                        stringChars.Append(SINGLEQUOTE);
                        thisReader = thisReader.Next();
                    }
                    else
                    {
                        // the closing single-quote character
                        isTerminated = true;
                        break;
                    }
                }
                else
                {
                    // we just read a literal string character
                    stringChars.Append(peek.Value);
                    thisReader = thisReader.Next();
                }
            }
            // make sure we found the end of the string
            if (!isTerminated)
            {
                throw new InvalidOperationException("Unterminated string found.");
            }
            // return the result
            var extent = SourceExtent.From(sourceChars);
            var stringValue = stringChars.ToString();
            return (new StringLiteralToken(extent, stringValue), thisReader);
        }

        private static (Token, SourceReader) ReadWhitespaceToken(SourceReader reader)
        {
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            // read the first whitespace character
            (sourceChar, thisReader) = thisReader.Read(ArmStringValidator.IsWhitespace);
            sourceChars.Add(sourceChar);
            // read the remaining whitespace
            while (!thisReader.Eof() && thisReader.Peek(ArmStringValidator.IsWhitespace))
            {
                (sourceChar, thisReader) = thisReader.Read();
                sourceChars.Add(sourceChar);
            }
            // return the result
            var extent = SourceExtent.From(sourceChars);
            return (new WhitespaceToken(extent), thisReader);
        }

        #endregion

    }

}
