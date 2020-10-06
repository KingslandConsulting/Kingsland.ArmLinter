using Kingsland.ArmLinter.Ast;
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

        public static IEnumerable<object> Evaluate(ArmArgumentListAst node)
        {
            foreach (var arg in node.ArgumentList)
            {
                yield return ArmExpressionEvaluator.Evaluate(arg);
            }
        }

        public static void Evaluate(ArmBracketedArgumentListAst node)
        {
            throw new NotImplementedException();
        }

        private static void Evaluate(ArmElementAccessExpressionAst node)
        {
            throw new NotImplementedException();
        }

        public static object Evaluate(ArmExpressionAst node)
        {
            return node switch
            {
                ArmStringLiteralExpressionAst n =>
                    ArmExpressionEvaluator.Evaluate(n),
                ArmInvocationExpressionAst n =>
                    ArmExpressionEvaluator.Evaluate(n),
                _ =>
                    throw new NotImplementedException()
            };
        }

        public static object Evaluate(ArmFunctionReferenceAst node, ArmArgumentListAst argList)
        {
            switch (node.Name.Name)
            {
                case "base64":
                    var inputString = (string)ArmExpressionEvaluator.Evaluate(argList).Single();
                    return ArmTemplateFunctions.String.Base64(inputString);
                case "base64ToString":
                    var base64Value = (string)ArmExpressionEvaluator.Evaluate(argList).Single();
                    return ArmTemplateFunctions.String.Base64ToString(base64Value);
                case "concat":
                    // check if this is the <string> or <object[]> version based on the argument types
                    var args = ArmExpressionEvaluator.Evaluate(argList).ToList();
                    if (args == null)
                    {
                        throw new InvalidOperationException($"Concat args cannot be null.");
                    }
                    else if (args.Count == 0)
                    {
                        throw new InvalidOperationException($"Concat must have at least one arg.");
                    }
                    else if (args.All(arg => arg is string))
                    {
                        return ArmTemplateFunctions.String.Concat(
                            args.Cast<string>().ToArray()
                        );
                    }
                    else if (args.All(arg => arg is object[]))
                    {
                        return ArmTemplateFunctions.Array.Concat(
                            args.Cast<object[]>().ToArray()
                        );
                    }
                    else
                    {
                        throw new InvalidOperationException($"Concat args must all be of type {typeof(string).Name} or all be of type {typeof(object[]).Name}.");
                    }
                case "toLower":
                    {
                        var stringToChange = (string)ArmExpressionEvaluator.Evaluate(argList).Single();
                        return ArmTemplateFunctions.String.ToLower(stringToChange);
                    }
                case "toUpper":
                    {
                        var stringToChange = (string)ArmExpressionEvaluator.Evaluate(argList).Single();
                        return ArmTemplateFunctions.String.ToUpper(stringToChange);
                    }
                default:
                    throw new NotImplementedException($"The ARM Template function '{node.Name.Name}' is not implemented.");
            };
        }

        public static object Evaluate(ArmInvocationExpressionAst node)
        {
            return node.Expression switch
            {
                ArmFunctionReferenceAst n =>
                    ArmExpressionEvaluator.Evaluate(n, node.ArgumentList),
                _ =>
                    throw new NotImplementedException()
            };
        }

        public static void Evaluate(ArmMemberAccessExpressionAst node)
        {
            throw new NotImplementedException();
        }

        public static void Evaluate(ArmNumericLiteralExpressionAst node)
        {
            throw new NotImplementedException();
        }

        public static string Evaluate(ArmStringLiteralExpressionAst node)
        {
            return node.Token.Value;
        }

        public static void Evaluate(ArmSubexpressionAst node)
        {
            throw new NotImplementedException();
        }

    }

}
