using System;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Intersection

        public static object[] IntersectionBinder(object[] functionArgs)
        {

            const string functionName = "intersection";

            // throw for incorrect number of arguments
            // e.g. "intersection()", "intersection('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if (functionArgs.Length < 2)
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function 'intersection'. " +
                    $"At least two parameter should be provided. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            // all args need to be the same type
            var expectedType = functionArgs[0] switch
            {
                int => typeof(int),
                string => typeof(string),
                object[] => typeof(object[]),
                _ => throw new InvalidOperationException()
            };
            if (functionArgs[1..].Any(
                arg => (arg == null) || (arg.GetType() != expectedType))
            )
            {
                throw new InvalidOperationException(
                    $"Template language function '{functionName}' expects parameters of the same type, but found multiple types."
                );
            }

            // all args need to be an array or
            // all args need to be an object
            if (functionArgs[1..].Any(
                arg => arg switch {
                    Array _ => false,
                    //object => false,
                    _ => true
                })
            )
            {
                throw new InvalidOperationException(
                    $"The template language function '{functionName}' expects either a comma separated list of arrays or a comma separated list of objects as its parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions#{functionName} for usage details."
                );
            }

            // unbundle the function arguments
            var arg1 = functionArgs[0];
            var arg2 = functionArgs[1];

            return ArmFunctions.Intersection(
                (object[])arg1,
                (object[])arg2
            );

        }

        public static object[] Intersection(params object[][] args)
        {
            if (args.Length == 0)
            {
                return Array.Empty<object>();
            }
            var result = args[0];
            foreach (var arg in args[1..])
            {
                result = result.Intersect(arg).ToArray();
            }
            return result;
        }

        #endregion

    }

}
