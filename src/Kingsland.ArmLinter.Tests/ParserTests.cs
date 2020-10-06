using Kingsland.ArmLinter.Ast;
using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Parsing;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Kingsland.ArmLinter.Tests
{
    public static class ParserTests
    {

        [Test]
        public static void ParseEmptyString()
        {
            var expression = "";
            var ex = Assert.Throws<UnexpectedEndOfStreamException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Exception of type 'Kingsland.ParseFx.Parsing.UnexpectedEndOfStreamException' was thrown.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        #region FunctionReference Tests

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


        [Test]
        public static void ParseFunctionMissingParens()
        {
            var expression = "concat";
            var ex = Assert.Throws<UnexpectedEndOfStreamException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Exception of type 'Kingsland.ParseFx.Parsing.UnexpectedEndOfStreamException' was thrown.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public static void ParseFunctionReferenceMissingParens()
        {
            var expression = "concat";
            var ex = Assert.Throws<UnexpectedEndOfStreamException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Exception of type 'Kingsland.ParseFx.Parsing.UnexpectedEndOfStreamException' was thrown.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public static void ParseFunctionReferenceUnclosedParens()
        {
            var expression = "concat(";
            var ex = Assert.Throws<UnexpectedEndOfStreamException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Exception of type 'Kingsland.ParseFx.Parsing.UnexpectedEndOfStreamException' was thrown.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public static void ParseFunctionReferenceUnclosedParensWithParameter()
        {
            var expression = "concat('storage'";
            var ex = Assert.Throws<UnexpectedEndOfStreamException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Exception of type 'Kingsland.ParseFx.Parsing.UnexpectedEndOfStreamException' was thrown.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public static void ParseFunctionReferenceUnclosedStringQuotes()
        {
            var expression = "concat('storage";
            var ex = Assert.Throws<InvalidOperationException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "Unterminated string found.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public static void ParseFunctionReferenceUnexpectedSequence()
        {
            var expression = "concat()'storage'";
            var ex = Assert.Throws<InvalidOperationException>(
                () => {
                    var actual = ArmExpressionParser.Parse(expression);
                }
            );
            var expectedMessage = "End of expression expected.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        #endregion

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
            var expected = new ArmElementAccessExpressionAst(
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