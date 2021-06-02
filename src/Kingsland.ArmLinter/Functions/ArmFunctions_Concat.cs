using System;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Concat

        public static object ConcatBinder(object[] functionArgs)
        {

            const string functionName = "concat";

            // throw for incorrect number of arguments
            // e.g. "concat()"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsAtLeastOne(
                functionName, functionArgs,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for mixed array and value arguments
            // e.g. "concat(createArray('hello', 'brave'), 'new'. 'world')"
            ArgHelper.AssertArgumentsAllArrayOrNoneArray(
                functionName, functionArgs,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            if (functionArgs[0].GetType().IsArray)
            {
                return ArmFunctions.Concat(functionArgs);
            }
            else
            {
                var stringArgs = functionArgs.Select(
                    arg => arg switch
                    {
                        string s => s,
                        int i => i.ToString(),
                        _ => throw new InvalidCastException()
                    }
                ).ToArray();
                return ArmFunctions.Concat(stringArgs);
            }

        }

        public static string Concat(params string[] args)
        {
            return string.Join(string.Empty, args);
        }

        public static object[] Concat(params object[] args)
        {
            return args.Cast<Array>()
                .SelectMany(arr => arr.Cast<object>())
                .ToArray();
        }

        #endregion

    }

}
