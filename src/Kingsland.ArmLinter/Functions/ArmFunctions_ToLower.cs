using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region ToLower

        public static string ToLowerBinder(object[] functionArgs)
        {

            const string functionName = "toLower";

            // throw for incorrect number of arguments
            // e.g. "toLower()", "toLower('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactlyOne_v2(
                functionName, functionArgs,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v1(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions"
            );

            // unbundle the function arguments
            var inputString = functionArgs[0];

            return ArmFunctions.ToLower((string)inputString);

        }

        /// <summary>
        /// Converts the specified string to lower case.
        /// </summary>
        /// <param name="stringToChange">The value to convert to lower case.</param>
        /// <returns>The string converted to lower case.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#tolower
        /// </remarks>
        /// <example>
        /// ToLower("One Two Three") => "one two three"
        /// </example>
        public static string ToLower(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToLowerInvariant();
        }

        #endregion

    }

}
