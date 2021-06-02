using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Trim

        public static string TrimBinder(object[] functionArgs)
        {

            const string functionName = "trim";

            // throw for incorrect number of arguments
            // e.g. "base64()", "base64('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v2(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v5(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions"
            );

            // unbundle the function arguments
            var inputString = functionArgs[0];

            return ArmFunctions.Trim((string)inputString);

        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the specified string.
        /// </summary>
        /// <returns>The string without leading and trailing white-space characters.</returns>
        /// <param name="stringToTrim">The value to trim.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#trim
        /// </remarks>
        /// <example>
        /// Take("    one two three   ") => "one two three"
        /// </example>
        public static string Trim(string stringToTrim)
        {
            if (stringToTrim == null)
            {
                throw new ArgumentNullException(nameof(stringToTrim));
            }
            return stringToTrim.Trim();
        }

        #endregion

    }

}
