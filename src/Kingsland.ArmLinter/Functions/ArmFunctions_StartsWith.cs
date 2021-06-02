using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region StartsWith


        public static bool StartsWithBinder(object[] functionArgs)
        {

            const string functionName = "startsWith";

            // throw for incorrect number of arguments
            // e.g. "startsWith()", "startsWith('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 2,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // validate the argument types
            ArgHelper.AssertArgumentTypes_v4(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) },
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            // unbundle the function arguments
            var stringToSearch = functionArgs[0];
            var stringToTest = functionArgs[1];

            return ArmFunctions.StartsWith(
                (string)stringToSearch, (string)stringToTest
            );

        }

        /// <summary>
        /// Determines whether a string starts with a value. The comparison is case-insensitive.
        /// </summary>
        /// <returns>True if the first character or characters of the string match the value; otherwise, False.</returns>
        /// <param name="stringToSearch">The value that contains the item to find.</param>
        /// <param name="stringToFind">The value to find.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#startswith
        /// </remarks>
        /// <example>
        /// StartsWith("abcdef", "ab") => true
        /// </example>
        /// <example>
        /// StartsWith("abcdef", "A") => true
        /// </example>
        /// <example>
        /// StartsWith("abcdef", "e") => false
        /// </example>
        public static bool StartsWith(string stringToSearch, string stringToFind)
        {
            if (stringToSearch == null)
            {
                throw new ArgumentNullException(nameof(stringToSearch));
            }
            if (stringToFind == null)
            {
                throw new ArgumentNullException(nameof(stringToFind));
            }
            return stringToSearch.StartsWith(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

    }

}
