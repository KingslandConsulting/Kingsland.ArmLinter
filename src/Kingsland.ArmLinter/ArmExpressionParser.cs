﻿using Kingsland.ArmLinter.Ast;
using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Parsing;
using Kingsland.ParseFx.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingsland.ArmLinter
{

    public static class ArmExpressionParser
    {

        public static ArmExpressionAst Parse(string armExpression)
        {
            var lexer = ArmExpressionLexer.Create();
            var tokens = lexer.Lex(armExpression);
            var ast = ArmExpressionParser.Parse(tokens);
            return ast;
        }

        public static ArmExpressionAst Parse(IEnumerable<SyntaxToken> lexerTokens)
        {

            // remove all whitespace
            var tokens = lexerTokens.Where(lt => !(lt is WhitespaceToken));

            var stream = new TokenStream(tokens);
            var program = ArmExpressionParser.ParseArmExpressionAst(stream);

            if (!stream.Eof)
            {
                throw new InvalidOperationException("End of expression expected.");
            }

            return program;

        }

        #region Parser Methods

        public static ArmExpressionAst ParseArmExpressionAst(TokenStream stream)
        {
            var peek = stream.Peek();
            return peek switch
            {
                OpenParenToken _ => ArmExpressionParser.ParseArmSubexpressionAst(stream),
                IdentifierToken _ => ArmExpressionParser.ParseDottedNotationExpressionAst(stream),
                StringLiteralToken _ => ArmExpressionParser.ParseArmStringLiteralExpressionAst(stream),
                IntegerToken _ => ArmExpressionParser.ParseArmNumericLiteralExpressionAst(stream),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// subexpression = "(" expression ")"
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ArmSubexpressionAst ParseArmSubexpressionAst(TokenStream stream)
        {
            // "("
            var openParen = stream.Read<OpenParenToken>();
            // expression
            var subexpression = ArmExpressionParser.ParseArmExpressionAst(stream);
            // ")"
            var closeParen = stream.Read<CloseParenToken>();
            // return the result
            return new ArmSubexpressionAst(subexpression);
        }

        /// <summary>
        /// invocation    = expression argument_list
        /// argument_list = "(" [ argument *( "," argument ) ] ")"
        /// argument      = expression
        ///
        /// member_access = expression operator member_name
        /// operator      = "."
        /// member_name   = identifier
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>
        ///
        ///     "resourceGroup().location" ->
        ///     + expression: member_access
        ///         + invocation
        ///           + expression: function_reference
        ///             + name: "resourceGroup"
        ///           + argument_list
        ///             + openParen: "("
        ///             + arguments: (empty)
        ///             + closeParen: ")"
        ///         + operator: "."
        ///         + name: "location"
        /// </remarks>
        public static ArmExpressionAst ParseDottedNotationExpressionAst(TokenStream stream)
        {
            var identifier = default(IdentifierToken);
            var expression = default(ArmExpressionAst);
            // the first expression has to be a function reference
            identifier = stream.Read<IdentifierToken>();
            expression = new ArmInvocationExpressionAst(
                new ArmFunctionReferenceAst(identifier),
                ArmExpressionParser.ParseArmArgumentListAst(stream)
            );
            // subsequent epressions must be dotted member access or array element access
            var loopAgain = true;
            while (loopAgain && !stream.Eof)
            {
                var peek = stream.Peek();
                switch (peek)
                {
                    case DotOperatorToken _:
                        var @operator = stream.Read<DotOperatorToken>();
                        identifier = stream.Read<IdentifierToken>();
                        expression = new ArmMemberAccessExpressionAst(
                            expression, @operator, identifier
                        );
                        break;
                    case OpenBracketToken _:
                        expression = new ArmElementAccessExpressionAst(
                            expression,
                            ArmExpressionParser.ParseArmBracketedArgumentListAst(stream)
                        );
                        break;
                    default:
                        loopAgain = false;
                        break;
                }
            }
            // return the result
            return expression;
        }

        /// <summary>
        /// function_call = function_name
        /// function_name   = identifier
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ArmFunctionReferenceAst ParseArmFunctionReferenceAst(TokenStream stream)
        {
            // function_name
            var name = stream.Read<IdentifierToken>();
            // return the result
            return new ArmFunctionReferenceAst(name);
        }

        /// <summary>
        /// argument_list = "[" [ argument *( "," argument ) ] "]"
        /// argument      = expression
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ArmArgumentListAst ParseArmArgumentListAst(TokenStream stream)
        {
            // "("
            var openParen = stream.Read<OpenParenToken>();
            // [ argument *( "," argument ) ]
            var arguments = new List<ArmExpressionAst>();
            if (!stream.TryPeek<CloseParenToken>())
            {
                // argument
                arguments.Add(
                    ArmExpressionParser.ParseArmExpressionAst(stream)
                );
                // *( "," argument )
                while (stream.TryRead<CommaToken>(out var comma))
                {
                    // argument
                    arguments.Add(
                        ArmExpressionParser.ParseArmExpressionAst(stream)
                    );
                }
            }
            // ")"
            var closeParen = stream.Read<CloseParenToken>();
            // return the result
            return new ArmArgumentListAst(openParen, arguments, closeParen);
        }

        public static ArmBracketedArgumentListAst ParseArmBracketedArgumentListAst(TokenStream stream)
        {
            // "["
            var openBracket = stream.Read<OpenBracketToken>();
            // [ argument *( "," argument ) ]
            var arguments = new List<ArmExpressionAst>();
            if (!stream.TryPeek<CloseBracketToken>())
            {
                // argument
                arguments.Add(
                    ArmExpressionParser.ParseArmExpressionAst(stream)
                );
                // *( "," argument )
                while (stream.TryRead<CommaToken>(out var comma))
                {
                    // argument
                    arguments.Add(
                        ArmExpressionParser.ParseArmExpressionAst(stream)
                    );
                }
            }
            // "]"
            var closeBracket = stream.Read<CloseBracketToken>();
            // return the result
            return new ArmBracketedArgumentListAst(openBracket, arguments, closeBracket);
        }

        /// <summary>
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ArmStringLiteralExpressionAst ParseArmStringLiteralExpressionAst(TokenStream stream)
        {
            // return the result
            return new ArmStringLiteralExpressionAst(
                stream.Read<StringLiteralToken>()
            );
        }

        /// <summary>
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ArmNumericLiteralExpressionAst ParseArmNumericLiteralExpressionAst(TokenStream stream)
        {
            var peek = stream.Peek();
            return peek switch
            {
                IntegerToken _ => new ArmNumericLiteralExpressionAst(
                                       stream.Read<IntegerToken>()
                                   ),
                _ => throw new InvalidOperationException(),
            };
        }

        #endregion

    }

}
