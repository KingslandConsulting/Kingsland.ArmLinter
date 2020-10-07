using Kingsland.ArmLinter.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

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

        private static TResult Invoke<T, TResult>(Func<T, TResult> func, List<object> args)
        {
            ArmExpressionEvaluator.ValidateArgs(new[] { typeof(T) }, args);
            return func.Invoke((T)args[0]);
        }

        private static TResult Invoke<T1, T2, TResult>(Func<T1, T2, TResult> func, List<object> args)
        {
            ArmExpressionEvaluator.ValidateArgs(new[] { typeof(T1), typeof(T2) }, args);
            return func.Invoke((T1)args[0], (T2)args[1]);
        }

        private static TResult Invoke<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, List<object> args)
        {
            ArmExpressionEvaluator.ValidateArgs(new[] { typeof(T1), typeof(T2), typeof(T3) }, args);
            return func.Invoke((T1)args[0], (T2)args[1], (T3)args[2]);
        }

        private static void ValidateArgs(Type[] types, List<object> args)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            if (types.Length != args.Count)
            {
                throw new InvalidOperationException($"'{nameof(types)}' must have the same number of items as '{nameof(args)}.");
            }
            for (var i = 0; i < types.Length; i++)
            {
                if (args[i].GetType() != types[i])
                {
                    throw new ArgumentException($"Argument {i} is of type '{args[i].GetType().Name}' but expected type '{types[i].Name}'");
                }
            }
        }

        private static object Evaluate(ArmFunctionReferenceAst node, ArmArgumentListAst argList)
        {
            var args = ArmExpressionEvaluator.Evaluate(argList).ToList();
            switch (node.Name.Name)
            {
                case "base64":
                    Func<string, string> base64 = (string inputString) =>
                        ArmTemplateFunctions.String.Base64(inputString);
                    return ArmExpressionEvaluator.Invoke(base64, args);
                case "base64ToString":
                    Func<string, string> base64ToString = (string base64Value) =>
                        ArmTemplateFunctions.String.Base64ToString(base64Value);
                    return ArmExpressionEvaluator.Invoke(base64ToString, args);
                case "concat":
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
                        Func<string[], string> concat_string = (string[] args) =>
                            ArmTemplateFunctions.String.Concat(args);
                        return ArmExpressionEvaluator.Invoke(
                            concat_string, new List<object> { args.Cast<string>().ToArray() }
                        );
                    }
                    else if (args.All(arg => arg is object[]))
                    {
                        Func<object[][], object[]> concat_array = (object[][] args) =>
                            ArmTemplateFunctions.Array.Concat(args);
                        return ArmExpressionEvaluator.Invoke(
                            concat_array, new List<object> { args.Cast<string[]>().ToArray() }
                        );
                    }
                    else
                    {
                        throw new InvalidOperationException($"Concat args must all be of type {typeof(string).Name} or all be of type {typeof(object[]).Name}.");
                    }
                case "padLeft":
                    switch (args.Count)
                    {
                        case 2: {
                            Func<string, int, string> padLeft = (string valueToPad, int totalLength) =>
                                ArmTemplateFunctions.String.PadLeft(valueToPad, totalLength);
                            return ArmExpressionEvaluator.Invoke(padLeft, args);
                        }
                        case 3: {
                            Func<string, int, char, string> padLeft = (string valueToPad, int totalLength, char paddingCharacter) =>
                                ArmTemplateFunctions.String.PadLeft(valueToPad, totalLength, paddingCharacter);
                            return ArmExpressionEvaluator.Invoke(padLeft, args);
                        }
                        default:
                            throw new InvalidOperationException();
                    }
                case "toLower":
                    Func<string, string> toLower = (string stringToChange) =>
                        ArmTemplateFunctions.String.ToLower(stringToChange);
                    return ArmExpressionEvaluator.Invoke(toLower, args);
                case "toUpper":
                    Func<string, string> toUpper = (string stringToChange) =>
                        ArmTemplateFunctions.String.ToUpper(stringToChange);
                    return ArmExpressionEvaluator.Invoke(toUpper, args);
                default:
                    throw new NotImplementedException($"The ARM Template function '{node.Name.Name}' is not implemented.");
            };
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

        public static long Evaluate(ArmNumericLiteralExpressionAst node)
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
