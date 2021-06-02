using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region ToUpper

        public static string ToUpperBinder(object[] functionArgs)
        {

            const string functionName = "toUpper";

            // throw for incorrect number of arguments
            // e.g. "toUpper()", "toUpper('one', 'two')"
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

            return ArmFunctions.ToUpper(
                (string)inputString
            );

        }

        /// <summary>
        /// Converts the specified string to upper case.
        /// </summary>
        /// <param name="stringToChange">The value to convert to upper case.</param>
        /// <returns>The string converted to upper case.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#toupper
        /// </remarks>
        /// <example>
        /// ToUpper("One Two Three") => "ONE TWO THREE"
        /// </example>
        public static string ToUpper(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToUpperInvariant();
        }

        #endregion

    }

}
