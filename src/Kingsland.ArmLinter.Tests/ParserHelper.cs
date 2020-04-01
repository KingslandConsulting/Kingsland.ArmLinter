using Kingsland.ArmLinter.Ast;
using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Syntax;
using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    internal static class ParserHelper
    {

        public static void AssertAreEqual(ArmSyntaxNodeAst expectedNode, ArmSyntaxNodeAst actualNode)
        {
            if ((expectedNode == null) && (actualNode == null))
            {
                return;
            }
            if (expectedNode == null)
            {
                Assert.Fail($"{nameof(expectedNode)} is null, but {nameof(actualNode)} is not null");
            }
            if (actualNode == null)
            {
                Assert.Fail($"{nameof(expectedNode)} is not null, but {nameof(actualNode)} is null");
            }
            Assert.AreEqual(
                expectedNode.GetType(), actualNode.GetType(),
                $"{nameof(expectedNode)} type does not match {nameof(actualNode)} type"
            );
            switch (expectedNode)
            {
                case ArmArgumentListAst expected:
                    {
                        var actual = (ArmArgumentListAst)actualNode;
                        Assert.AreEqual(expected.ArgumentList.Count, actual.ArgumentList.Count);
                        for (var i = 0; i < expected.ArgumentList.Count; i++)
                        {
                            ParserHelper.AssertAreEqual(expected.ArgumentList[i], actual.ArgumentList[i]);
                        }
                    }
                    break;
                case ArmBracketedArgumentListAst expected:
                    {
                        var actual = (ArmBracketedArgumentListAst)actualNode;
                        Assert.AreEqual(expected.ArgumentList.Count, actual.ArgumentList.Count);
                        for (var i = 0; i < expected.ArgumentList.Count; i++)
                        {
                            ParserHelper.AssertAreEqual(expected.ArgumentList[i], actual.ArgumentList[i]);
                        }
                    }
                    break;
                case ArmElementExpressionAst expected:
                    {
                        var actual = (ArmElementExpressionAst)actualNode;
                        ParserHelper.AssertAreEqual(expected.Expression, actual.Expression);
                        ParserHelper.AssertAreEqual(expected.ArgumentList, actual.ArgumentList);
                    }
                    break;
                case ArmFunctionReferenceAst expected:
                    {
                        var actual = (ArmFunctionReferenceAst)actualNode;
                        Assert.AreEqual(expected.Name.Name, actual.Name.Name);
                    }
                    break;
                case ArmInvocationExpressionAst expected:
                    {
                        var actual = (ArmInvocationExpressionAst)actualNode;
                        ParserHelper.AssertAreEqual(expected.Expression, actual.Expression);
                        ParserHelper.AssertAreEqual(expected.ArgumentList, actual.ArgumentList);
                    }
                    break;
                case ArmMemberAccessExpressionAst expected:
                    {
                        var actual = (ArmMemberAccessExpressionAst)actualNode;
                        ParserHelper.AssertAreEqual(expected.Expression, actual.Expression);
                        switch (expected.OperatorToken)
                        {
                            case DotOperatorToken _:
                                Assert.IsInstanceOf(expected.OperatorToken.GetType(), actual.OperatorToken);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        Assert.AreEqual(expected.Name.Name, actual.Name.Name);
                    }
                    break;
                case ArmNumericLiteralExpressionAst expected:
                    {
                        var actual = (ArmNumericLiteralExpressionAst)actualNode;
                        switch (expected.Token)
                        {
                            case IntegerToken expectedToken:
                                Assert.IsInstanceOf(expectedToken.GetType(), actual.Token);
                                var actualToken = (IntegerToken)actual.Token;
                                Assert.AreEqual(expectedToken.Value, actualToken.Value);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;
                case ArmStringLiteralExpressionAst expected:
                    {
                        var actual = (ArmStringLiteralExpressionAst)actualNode;
                        Assert.AreEqual(expected.Token.Value, actual.Token.Value);
                    }
                    break;
                default:
                    throw new NotImplementedException(
                        $"Cannot compare expected type '{expectedNode.GetType().Name}' of {nameof(expectedNode)}"
                    );
            }
        }

        public static void AssertAreEqual(SyntaxToken expectedToken, SyntaxToken actualToken)
        {
            if ((expectedToken == null) && (actualToken == null))
            {
                return;
            }
            if (expectedToken == null)
            {
                Assert.Fail($"{nameof(expectedToken)} is null, but {nameof(actualToken)} is not null");
            }
            if (actualToken == null)
            {
                Assert.Fail($"{nameof(expectedToken)} is not null, but {nameof(actualToken)} is null");
            }
            Assert.AreEqual(
                expectedToken.GetType(), actualToken.GetType(),
                $"{nameof(expectedToken)} type does not match {nameof(actualToken)} type"
            );
            switch (expectedToken)
            {
                default:
                    throw new NotImplementedException(
                        $"Cannot compare expected type '{expectedToken.GetType().Name}' of {nameof(expectedToken)}"
                    );
            }
        }

    }

}
