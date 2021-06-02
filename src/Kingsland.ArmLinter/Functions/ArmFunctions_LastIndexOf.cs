using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region LastIndexOf

        public static int LastIndexOfBinder(object[] functionArgs)
        {

            const string functionName = "lastIndexOf";

            // throw for incorrect number of arguments
            // e.g. "lastIndexOf()", "lastIndexOf('one', 'two', 'three')"
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

            return ArmFunctions.LastIndexOf(
                (string)stringToSearch,
                (string)stringToFind
            );

        }

        /// <summary>
        /// Returns the last position of a value within a string.
        /// The comparison is case-insensitive.
        /// </summary>
        /// <returns>An integer that represents the last position of the item to find.
        /// The value is zero-based. If the item isn't found, -1 is returned.</returns>
        /// <param name="stringToSearch">The value that contains the item to find.</param>
        /// <param name="stringToFind">The value to find.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#lastindexof
        /// </remarks>
        /// <example>
        /// LastIndexOf("test", "t") => 3
        /// </example>
        /// <example>
        /// LastIndexOf("abcdef", "AB") => 0
        /// </example>
        /// <example>
        /// LastIndexOf("abcdef", "z") => -1
        /// </example>
        public static int LastIndexOf(string stringToSearch, string stringToFind)
        {
            if (stringToSearch == null)
            {
                throw new ArgumentNullException(nameof(stringToSearch));
            }
            if (stringToFind == null)
            {
                throw new ArgumentNullException(nameof(stringToFind));
            }
            if (stringToFind == string.Empty)
            {
                // arm templates return "length - 1", but dotnet returns "length"
                // (e.g. "lastIndexOf('abcdef', '')" = 5), *except* if stringToSearch
                // is an empty string as well, in which case arm templates return
                // 0 (i.e. "lastIndexOf('', '')" = 0)
                return (stringToSearch == string.Empty)
                    ? 0
                    : stringToSearch.Length - 1;
            }
            return stringToSearch.LastIndexOf(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

    }

}
