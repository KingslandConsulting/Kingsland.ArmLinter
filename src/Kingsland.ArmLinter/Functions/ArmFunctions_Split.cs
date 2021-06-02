using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Split

        public static object[] SplitBinder(object[] functionArgs)
        {

            const string functionName = "split";

            // throw for incorrect number of arguments
            // e.g. "split()", "split('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v2(
                functionName, functionArgs, 2,
                $"https://aka.ms/arm-template-expressions"
            );

            // if delimiters is object[] we need to convert it to string[]
            var newFunctionArgs = new[] {
                functionArgs[0], functionArgs[1]
            };
            if (newFunctionArgs[1] is object[] delimiterObjectArray)
            {
                if (BindingHelper.TryConvertArray(
                    delimiterObjectArray, typeof(string), out var delimiterStringArray
                ))
                {
                    newFunctionArgs[1] = delimiterStringArray;
                }
            }

            // validate the argument types
            ArgHelper.AssertArgumentTypes_v5(
                functionName, newFunctionArgs,
                new Type[][] {
                    new[] { typeof(string) },
                    new[] { typeof(string), typeof(string[]) }
                },
                $"https://aka.ms/arm-template-expressions"
            );

            // unbundle the function arguments
            var inputString = newFunctionArgs[0];
            var delimiter = newFunctionArgs[1];

            return delimiter switch
            {
                string str =>
                    ArmFunctions.Split(
                        (string)inputString, str
                    ),
                object[] obj =>
                    ArmFunctions.Split(
                        (string)inputString, (string[])delimiter
                    ),
                _ =>
                    throw new InvalidOperationException()
            };

        }

        /// <summary>
        /// Returns an array of strings that contains the substrings of the input string that are delimited by the specified delimiters.
        /// </summary>
        /// <returns>An array of strings.</returns>
        /// <param name="inputString">The string to split.</param>
        /// <param name="delimiter">The delimiters to use for splitting the string.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#split
        /// </remarks>
        /// <example>
        /// Split("one,two,three", ",") => new string[] { "one", "two", "three" }
        /// </example>
        /// <example>
        /// Split("one;two,three", new string[] { ",", ";" }) => new string[] { "one", "two", "three" }
        /// </example>
        public static string[] Split(string inputString, string delimiter)
        {
            return ArmFunctions.Split(
                inputString, new[] { delimiter }
            );
        }

        /// <summary>
        /// Returns an array of strings that contains the substrings of the input string that are delimited by the specified delimiters.
        /// </summary>
        /// <returns>An array of strings.</returns>
        /// <param name="inputString">The string to split.</param>
        /// <param name="delimiter">The delimiters to use for splitting the string.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#split
        /// </remarks>
        /// <example>
        /// Split("one,two,three", ",") => new string[] { "one", "two", "three" }
        /// </example>
        /// <example>
        /// Split("one;two,three", new string[] { ",", ";" }) => new string[] { "one", "two", "three" }
        /// </example>
        public static string[] Split(string inputString, string[] delimiter)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            if (delimiter == null)
            {
                throw new ArgumentNullException(nameof(delimiter));
            }
            return inputString.Split(
                delimiter,
                StringSplitOptions.None
            );
        }

        #endregion

    }

}
