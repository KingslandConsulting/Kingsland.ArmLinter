using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Skip

        public static string SkipBinder(object[] functionArgs)
        {

            const string functionName = "skip";

            // throw for incorrect number of arguments
            // e.g. "skip()", "skip('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if (functionArgs.Length != 2)
            {
                // there's a typo in the azure arm api - someone must have done a search
                // and replace on the "take" function code to create the "skip" function,
                // and inadvertently changed the "take" verb in the error message to "skip"
                // in the processes. we'll reproduce the typo here as well for the sake of
                // consistency.
                var typoWord = "skip"; // should be "take"
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects exactly two parameters: " +
                    $"the collection to {typoWord} the objects from as the first parameter and " +
                    $"the count of objects to {typoWord} as the second parameter. " +
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
            var numberToSkip = functionArgs[1];

            return originalValue switch
            {
                string str =>
                    ArmFunctions.Skip(str, (int)numberToSkip),
                _ =>
                    throw new InvalidOperationException()
            };

        }

        /// <summary>
        /// Returns a string with all the characters after the specified number of characters.
        /// </summary>
        /// <returns>A string.</returns>
        /// <param name="originalValue">The string to use for skipping.</param>
        /// <param name="numberToSkip">
        /// The number of characters to skip. If this value is 0 or less, all the
        /// elements or characters in the value are returned. If it's larger than
        /// the length of the string, an empty string is returned.
        /// </param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#skip
        /// </remarks>
        /// <example>
        /// Skip("one two three", 4) => "two three"
        /// </example>
        public static string Skip(string originalValue, int numberToSkip)
        {
            if (originalValue == null)
            {
                throw new ArgumentNullException(nameof(originalValue));
            }
            return originalValue.Substring(
                Math.Max(0, Math.Min(originalValue.Length, numberToSkip))
            );
        }

        #endregion

    }

}
