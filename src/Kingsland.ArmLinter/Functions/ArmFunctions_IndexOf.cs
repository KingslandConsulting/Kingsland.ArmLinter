using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region IndexOf

        public static int IndexOfBinder(object[] functionArgs)
        {

            const string functionName = "indexOf";

            // throw for incorrect number of arguments
            // e.g. "indexOf()", "indexOf('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 2,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // throw for invalid argument types
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
            var stringToFind = functionArgs[1];

            return ArmFunctions.IndexOf(
                (string)stringToSearch,
                (string)stringToFind
            );

        }

        /// <summary>
        /// Returns the first position of a value within a string.
        /// The comparison is case-insensitive.
        /// </summary>
        /// <returns>An integer that represents the position of the item to find.
        /// The value is zero-based. If the item isn't found, -1 is returned.</returns>
        /// <param name="stringToSearch">The value that contains the item to find.</param>
        /// <param name="stringToFind">The value to find.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#indexof
        /// </remarks>
        /// <example>
        /// IndexOf("test", "t") => 0
        /// </example>
        /// <example>
        /// IndexOf("abcdef", "CD") => 2
        /// </example>
        /// <example>
        /// IndexOf("abcdef", "z") => -1
        /// </example>
        public static int IndexOf(string stringToSearch, string stringToFind)
        {
            if (stringToSearch == null)
            {
                throw new ArgumentNullException(nameof(stringToSearch));
            }
            if (stringToFind == null)
            {
                throw new ArgumentNullException(nameof(stringToFind));
            }
            return stringToSearch.IndexOf(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

    }

}
