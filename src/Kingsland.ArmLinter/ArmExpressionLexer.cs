using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Text;
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
                .AddScanner('[', ArmExpressionLexer.ScanOpenBracketToken)
                .AddScanner(']', ArmExpressionLexer.ScanCloseBracketToken)
                .AddScanner('(', ArmExpressionLexer.ScanOpenParenToken)
                .AddScanner(')', ArmExpressionLexer.ScanCloseParenToken)
                .AddScanner(',', ArmExpressionLexer.ScanCommaToken)
                .AddScanner('.', ArmExpressionLexer.ScanDotOperatorToken)
                .AddScanner('\'', ArmExpressionLexer.ScanStringLiteralToken)
                .AddScanner("[a-z|A-z|_]", ArmExpressionLexer.ScanIdentifierToken)
                .AddScanner("[+|\\-|0-9]", ArmExpressionLexer.ScanIntegerToken)
                .AddScanner(
                    new char[] { '\u0020', '\u000D', '\u000A' },
                    ArmExpressionLexer.ScanWhitespaceToken
                );
        }

        #endregion

        #region Token Scanner Methods

        private static ScannerResult ScanOpenBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('[');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new OpenBracketToken(extent), nextReader);
        }

        private static ScannerResult ScanCloseBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(']');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new CloseBracketToken(extent), nextReader);
        }

        private static ScannerResult ScanOpenParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('(');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new OpenParenToken(extent), nextReader);
        }

        private static ScannerResult ScanCloseParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(')');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new CloseParenToken(extent), nextReader);
        }

        private static ScannerResult ScanCommaToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(',');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new CommaToken(extent), nextReader);
        }

        private static ScannerResult ScanDotOperatorToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('.');
            var extent = SourceExtent.From(sourceChar);
            return new ScannerResult(new DotOperatorToken(extent), nextReader);
        }

        private static ScannerResult ScanIdentifierToken(SourceReader reader)
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
            return new ScannerResult(
                new IdentifierToken(extent, name), thisReader
            );
        }

        private static ScannerResult ScanIntegerToken(SourceReader reader)
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
            return new ScannerResult(
                new IntegerToken(extent, sign * value), thisReader
            );
        }

        private static ScannerResult ScanStringLiteralToken(SourceReader reader)
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
            return new ScannerResult(
                new StringLiteralToken(extent, stringValue), thisReader
            );
        }

        private static ScannerResult ScanWhitespaceToken(SourceReader reader)
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
            var value = extent.ToString();
            return new ScannerResult(
                new WhitespaceToken(extent, value), thisReader
            );
        }

        #endregion

    }

}
