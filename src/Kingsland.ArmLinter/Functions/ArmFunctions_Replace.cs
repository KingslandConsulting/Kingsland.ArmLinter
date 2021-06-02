using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Replace


        public static string ReplaceBinder(object[] functionArgs)
        {

            const string functionName = "replace";

            // throw for incorrect number of arguments
            // e.g. "replace()", "replace('one', 'two', 'three', 'four')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v2(
                functionName, functionArgs, 3,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v5(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) },
                    new[] { typeof(string) },
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions"
            );

            // unbundle the function arguments
            var originalString = functionArgs[0];
            var oldString = functionArgs[1];
            var newString = functionArgs[2];

            return ArmFunctions.Replace(
                (string)originalString,
                (string)oldString,
                (string)newString
            );

        }

        /// <summary>
         /// Returns a new string with all instances of one string replaced by another string.
         /// </summary>
         /// <returns>A string with the replaced characters.</returns>
         /// <param name="originalString">The value that has all instances of one string replaced by another string.</param>
         /// <param name="oldString">The string to be removed from the original string.</param>
         /// <param name="newString">	The string to add in place of the removed string.</param>
         /// <remarks>
         /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#replace
         /// </remarks>
         /// <example>
         /// Replace("123-123-1234", "-", "") => "1231231234"
         /// </example>
         /// <example>
         /// Replace("123-123-1234", "1234", "xxxx") => "123-123-xxxx"
         /// </example>
        public static string Replace(string originalString, string oldString, string newString)
        {
            if (originalString == null)
            {
                throw new ArgumentNullException(nameof(originalString));
            }
            return originalString.Replace(oldString, newString, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

    }

}
