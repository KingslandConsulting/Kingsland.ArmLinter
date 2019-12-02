using Kingsland.ArmLinter.Tokens;
using Kingsland.Lexing;
using Kingsland.Lexing.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsland.ArmLinter
{

    public sealed class ArmExpressionLexer : Lexer
    {

        #region Constructor

        public ArmExpressionLexer(SourceReader reader)
            : base(reader)
        {
        }

        #endregion

        #region Helpers

        public static List<Token> Lex(string sourceText)
        {
            var reader = SourceReader.From(sourceText);
            return new ArmExpressionLexer(reader).ReadToEnd().ToList();
        }

        #endregion

        #region Dispatcher

        public override (Token Token, Lexer NextLexer) ReadToken()
        {

            var reader = this.Reader;
            var peek = reader.Peek();

            // see https://docs.microsoft.com/en-us/azure/azure-resource-manager/template-expressions

            switch (peek.Value)
            {

                case '[':
                    return ArmExpressionLexer.ReadOpenBracketToken(reader);
                case ']':
                    return ArmExpressionLexer.ReadCloseBracketToken(reader);
                case '(':
                    return ArmExpressionLexer.ReadOpenParenToken(reader);
                case ')':
                    return ArmExpressionLexer.ReadCloseParenToken(reader);
                case ',':
                    return ArmExpressionLexer.ReadCommaToken(reader);
                case '.':
                    return ArmExpressionLexer.ReadDotOperatorToken(reader);
                case '\'':
                    return ArmExpressionLexer.ReadStringLiteralToken(reader);

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    return ArmExpressionLexer.ReadIdentifierToken(reader);

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                //case '+':
                case '-':
                    return ArmExpressionLexer.ReadIntegerToken(reader);

                case '\u0020': // space
                //case '\u0009': // horizontal tab
                case '\u000D': // carriage return
                case '\u000A': // line feed
                    return ArmExpressionLexer.ReadWhitespaceToken(reader);

                default:
                    throw new UnexpectedCharacterException(peek);

            }

        }

        #endregion

        #region Lexer Methods

        public static (OpenBracketToken, Lexer) ReadOpenBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('[');
            var extent = SourceExtent.From(sourceChar);
            return (new OpenBracketToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (CloseBracketToken, Lexer) ReadCloseBracketToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(']');
            var extent = SourceExtent.From(sourceChar);
            return (new CloseBracketToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (OpenParenToken, Lexer) ReadOpenParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('(');
            var extent = SourceExtent.From(sourceChar);
            return (new OpenParenToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (CloseParenToken, Lexer) ReadCloseParenToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(')');
            var extent = SourceExtent.From(sourceChar);
            return (new CloseParenToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (CommaToken, Lexer) ReadCommaToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read(',');
            var extent = SourceExtent.From(sourceChar);
            return (new CommaToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (DotOperatorToken, Lexer) ReadDotOperatorToken(SourceReader reader)
        {
            (var sourceChar, var nextReader) = reader.Read('.');
            var extent = SourceExtent.From(sourceChar);
            return (new DotOperatorToken(extent), new ArmExpressionLexer(nextReader));
        }

        public static (IdentifierToken, Lexer) ReadIdentifierToken(SourceReader reader)
        {
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            var nameChars = new StringBuilder();
            // identifierChar
            (sourceChar, thisReader) = thisReader.Read(ArmStringValidator.IsIdentifierChar);
            sourceChars.Add(sourceChar);
            nameChars.Append(sourceChar.Value);
            // *( identifierChar )
            while (!thisReader.Eof() && thisReader.Peek(ArmStringValidator.IsIdentifierChar))
            {
                (sourceChar, thisReader) = thisReader.Read();
                sourceChars.Add(sourceChar);
                nameChars.Append(sourceChar.Value);
            }
            // return the result
            var extent = SourceExtent.From(sourceChars);
            var name = nameChars.ToString();
            return (new IdentifierToken(extent, name), new ArmExpressionLexer(thisReader));
        }

        public static (IntegerToken, Lexer) ReadIntegerToken(SourceReader reader)
        {
            var thisReader = reader;
            var sourceChar = default(SourceChar);
            var sourceChars = new List<SourceChar>();
            var sign = 1;
            var value = 0;
            // read the sign
            if (thisReader.Peek().Value == '-')
            {
                sign = -1;
                (sourceChar, thisReader) = thisReader.Read('-');
                sourceChars.Add(sourceChar);
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
            return (new IntegerToken(extent, value * sign), new ArmExpressionLexer(thisReader));
        }

        public static (StringLiteralToken, Lexer) ReadStringLiteralToken(SourceReader reader)
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
            return (new StringLiteralToken(extent, stringValue), new ArmExpressionLexer(thisReader));
        }

        public static (WhitespaceToken, Lexer) ReadWhitespaceToken(SourceReader reader)
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
            return (new WhitespaceToken(extent), new ArmExpressionLexer(thisReader));
        }

        #endregion

    }

}
