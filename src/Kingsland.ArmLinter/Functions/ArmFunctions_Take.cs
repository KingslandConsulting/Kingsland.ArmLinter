using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Take

        public static string TakeBinder(object[] functionArgs)
        {

            const string functionName = "take";

            // throw for incorrect number of arguments
            // e.g. "take()", "take('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if (functionArgs.Length != 2)
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects exactly two parameters: " +
                    $"the collection to take the objects from as the first parameter and " +
                    $"the count of objects to take as the second parameter. " +
                    $"The function was invoked with '{functionArgs.Length}' parameter(s). " +
                    $"Please see https://aka.ms/arm-template-expressions#{functionName} for usage details."
                );
            }

            // throw for invalid argument types
            if ((functionArgs[0] is not Array) &&
                (functionArgs[0] is not string))
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its first parameter 'collection' to be an array or a string. " +
                    $"The provided value is of type '{ArgHelper.GetCapitalizedTypeName(functionArgs[0].GetType())}'. " +
                    $"Please see https://aka.ms/arm-template-expressions#{functionName} for usage details."
                );
            }
            if (functionArgs[1] is not int)
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its second parameter 'count' to be an integer. " +
                    $"The provided value is of type '{ArgHelper.GetCapitalizedTypeName(functionArgs[1].GetType())}'. " +
                    $"Please see https://aka.ms/arm-template-expressions#{functionName} for usage details."
                );
            }

            // unbundle the function arguments
            var originalValue = functionArgs[0];
            var numberToTake = functionArgs[1];

            return originalValue switch
            {
                string str =>
                    ArmFunctions.Take(
                        str,
                        (int)numberToTake
                    ),
                _ =>
                    throw new InvalidOperationException()
            };

        }

        /// <summary>
        /// Returns a string with the specified number of characters from the start of the string.
        /// </summary>
        /// <returns>A string.</returns>
        /// <param name="originalValue">The string to take the elements from.</param>
        /// <param name="numberToTake">
        /// The number of elements or characters to take. If this value is 0 or less,
        /// an empty array or string is returned. If it's larger than the length of
        /// the given array or string, all the elements in the array or string are
        /// </param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#take
        /// </remarks>
        /// <example>
        /// Take("one two three", 2) => "on"
        /// </example>
        public static string Take(string originalValue, int numberToTake)
        {
            if (originalValue == null)
            {
                throw new ArgumentNullException(nameof(originalValue));
            }
            return originalValue.Substring(
                0, Math.Max(0, Math.Min(originalValue.Length, numberToTake))
            );
        }

        #endregion

    }

}
