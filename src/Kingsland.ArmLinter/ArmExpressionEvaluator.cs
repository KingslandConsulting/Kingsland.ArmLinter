using Kingsland.ArmLinter.Ast;
using Kingsland.ArmLinter.Functions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingsland.ArmLinter
{

    public static class ArmExpressionEvaluator
    {

        public static object Evaluate(string expression)
        {
            var ast = ArmExpressionParser.Parse(expression);
            var value  = ArmExpressionEvaluator.Evaluate(ast);
            return value;
        }

        private static IEnumerable<object> Evaluate(ArmArgumentListAst node)
        {
            foreach (var arg in node.ArgumentList)
            {
                yield return ArmExpressionEvaluator.Evaluate(arg);
            }
        }

        private static void Evaluate(ArmBracketedArgumentListAst node)
        {
            throw new NotImplementedException();
        }

        private static void Evaluate(ArmElementAccessExpressionAst node)
        {
            throw new NotImplementedException();
        }

        private static object Evaluate(ArmExpressionAst node)
        {
            return node switch
            {
                ArmInvocationExpressionAst n =>
                    ArmExpressionEvaluator.Evaluate(n),
                ArmNumericLiteralExpressionAst n =>
                    ArmExpressionEvaluator.Evaluate(n),
                ArmStringLiteralExpressionAst n =>
                    ArmExpressionEvaluator.Evaluate(n),
                _ =>
                    throw new NotImplementedException()
            };
        }

        private static object Evaluate(ArmFunctionReferenceAst node, ArmArgumentListAst argList)
        {
            var args = ArmExpressionEvaluator.Evaluate(argList).ToArray();
            return BindingHelper.InvokeFunction(node.Name.Name, args);
        }

        private static object Evaluate(ArmInvocationExpressionAst node)
        {
            return node.Expression switch
            {
                ArmFunctionReferenceAst n =>
                    ArmExpressionEvaluator.Evaluate(n, node.ArgumentList),
                _ =>
                    throw new NotImplementedException()
            };
        }

        private static void Evaluate(ArmMemberAccessExpressionAst node)
        {
            throw new NotImplementedException();
        }

        public static int Evaluate(ArmNumericLiteralExpressionAst node)
        {
            return node.Token.Value;
        }

        private static string Evaluate(ArmStringLiteralExpressionAst node)
        {
            return node.Token.Value;
        }

        private static void Evaluate(ArmSubexpressionAst node)
        {
            throw new NotImplementedException();
        }

    }

}
