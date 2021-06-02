using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region First

        public static object FirstBinder(object[] functionArgs)
        {

            const string functionName = "first";

            // throw for incorrect number of arguments
            // e.g. "first()", "first('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v3_1(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(object[]), typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            // unbundle the function arguments
            var arg1 = functionArgs[0];

            switch( arg1)
            {
                case string str:
                    return ArmFunctions.First(str);
                case object[] arr:
                    if (arr.Length == 0)
                    {
                        throw new InvalidOperationException(
                            $"Template output JToken type is not valid. Expected 'Object'. Actual 'Null'." +
                            $"Please see https://aka.ms/arm-template/#outputs for usage details."
                        );
                    }
                    return ArmFunctions.First(arr);
                default:
                    throw new InvalidOperationException();
            };

        }

        /// <summary>
        /// Returns the first character of the string.
        /// </summary>
        /// <returns>The first character of the string.</returns>
        /// <param name="arg1">The value to retrieve the first character.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#first
        /// </remarks>
        /// <example>
        /// First("One Two Three") => "O"
        /// </example>
        public static string First(string arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return (arg1.Length == 0) ?
                string.Empty :
                new string(arg1[0], 1);
        }

        /// <summary>
        /// Returns the first element of the string.
        /// </summary>
        /// <returns>The first element in an array.</returns>
        /// <param name="arg1">The value to retrieve the first element.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#first
        /// </remarks>
        /// <example>
        /// First("One Two Three") => "O"
        /// </example>
        public static object First(object[] arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return (arg1.Length == 0) ?
                null :
                arg1[0];
        }

        #endregion

    }

}
