using Kingsland.ArmLinter.Ast;
using Kingsland.ArmLinter.Tokens;
using NUnit.Framework;
using System.Collections.Generic;

namespace Kingsland.ArmLinter.Tests
{
    public static class ParserTests
    {

        [Test]
        public static void ParseFunctionReferenceNoArguments()
        {
            var expression = "concat()";
            var actual = ArmExpressionParser.Parse(expression);
            var expected = new ArmInvocationExpressionAst(
                expression: new ArmFunctionReferenceAst(
                    name: new IdentifierToken("concat")
                ),
                argumentList: new ArmArgumentListAst(
                    openParen: new OpenParenToken(),
                    arguments: new List<ArmExpressionAst>(),
                    closeParen: new CloseParenToken()
                )
            );
            ParserHelper.AssertAreEqual(expected, actual);
            Assert.AreEqual(expression, actual.ToArmText());
        }

        [Test()]
        public static void ParseFunctionReferenceOneArgument()
        {
            var expression = "concat('aaa')";
            var actual = ArmExpressionParser.Parse(expression);
            var expected = new ArmInvocationExpressionAst(
                expression: new ArmFunctionReferenceAst(
                    name: new IdentifierToken("concat")
                ),
                argumentList: new ArmArgumentListAst(
                    openParen: new OpenParenToken(),
                    arguments: new List<ArmExpressionAst>() {
                        new ArmStringLiteralExpressionAst(
                            new StringLiteralToken("aaa")
                        )
                    },
                    closeParen: new CloseParenToken()
                )
            );
            ParserHelper.AssertAreEqual(expected, actual);
            Assert.AreEqual(expression, actual.ToArmText());
        }

        [Test()]
        public static void ParseFunctionReferenceTwoArguments()
        {
            var expression = "concat('aaa', 'bbb')";
            var actual = ArmExpressionParser.Parse(expression);
            var expected = new ArmInvocationExpressionAst(
                expression: new ArmFunctionReferenceAst(
                    name: new IdentifierToken("concat")
                ),
                argumentList: new ArmArgumentListAst(
                    openParen: new OpenParenToken(),
                    arguments: new List<ArmExpressionAst>() {
                        new ArmStringLiteralExpressionAst(
                            new StringLiteralToken("aaa")
                        ),
                        new ArmStringLiteralExpressionAst(
                            new StringLiteralToken("bbb")
                        )
                    },
                    closeParen: new CloseParenToken()
                )
            );
            ParserHelper.AssertAreEqual(expected, actual);
            Assert.AreEqual(expression, actual.ToArmText());
        }

        [Test()]
        public static void ParseMemberAccess()
        {
            var expression = "resourceGroup().id";
            var actual = ArmExpressionParser.Parse(expression);
            var expected = new ArmMemberAccessExpressionAst(
                expression: new ArmInvocationExpressionAst(
                    expression: new ArmFunctionReferenceAst(
                        name: new IdentifierToken("resourceGroup")
                    ),
                    argumentList: new ArmArgumentListAst(
                        openParen: new OpenParenToken(),
                        arguments: new List<ArmExpressionAst>(),
                        closeParen: new CloseParenToken()
                    )
                ),
                operatorToken: new DotOperatorToken(),
                name: new IdentifierToken("id")
            );
            ParserHelper.AssertAreEqual(expected, actual);
            Assert.AreEqual(expression, actual.ToArmText());
        }

        [Test()]
        public static void ParseElementExpression()
        {
            var expression = "providers('Microsoft.Storage', 'storageAccounts').apiVersions[0]";
            var actual = ArmExpressionParser.Parse(expression);
            var expected = new ArmElementExpressionAst(
                expression: new ArmMemberAccessExpressionAst(
                    expression: new ArmInvocationExpressionAst(
                        expression: new ArmFunctionReferenceAst(
                            name: new IdentifierToken("providers")
                        ),
                        argumentList: new ArmArgumentListAst(
                            openParen: new OpenParenToken(),
                            arguments: new List<ArmExpressionAst>() {
                                new ArmStringLiteralExpressionAst(
                                    new StringLiteralToken("Microsoft.Storage")
                                ),
                                new ArmStringLiteralExpressionAst(
                                    new StringLiteralToken("storageAccounts")
                                )
                            },
                            closeParen: new CloseParenToken()
                        )),
                    operatorToken: new DotOperatorToken(),
                    name: new IdentifierToken("apiVersions")
                ),
                argumentList: new ArmBracketedArgumentListAst(
                    openBracket: new OpenBracketToken(),
                    arguments: new List<ArmExpressionAst>() {
                        new ArmNumericLiteralExpressionAst(
                            new IntegerToken(0)
                        ),
                    },
                    closeBracket: new CloseBracketToken()
                )
            );
            ParserHelper.AssertAreEqual(expected, actual);
            Assert.AreEqual(expression, actual.ToArmText());
        }

    }

}