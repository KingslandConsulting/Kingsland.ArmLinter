using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region PadLeft

        public static string PadLeftBinder(object[] functionArgs)
        {

            const string functionName = "padLeft";

            // throw for incorrect number of arguments
            // e.g. "padLeft()", "padLeft('one', 'two', 'three', 'four')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if ((functionArgs.Length < 2) || (functionArgs.Length > 3))
            {
                throw new InvalidOperationException(
                    $"Unable to evaluate template language function '{functionName}': " +
                    $"function requires minimum two and maximum three arguments while '{functionArgs.Length}' was provided. " +
                    $"The syntax is {functionName}(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            // unbundle the function arguments

            var valueToPad = functionArgs[0];
            if ((valueToPad is not string) &&
               (valueToPad is not int))
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function '{functionName}': the first parameter is invalid. " +
                    $"The source string must be a String or integer type, while '{ArgHelper.GetCapitalizedTypeName(functionArgs[0].GetType())}' was provided. " +
                    $"The syntax is {functionName}(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            var totalLength = (int)functionArgs[1];
            if ((totalLength < 1) || (totalLength > 16))
            {
                throw new InvalidOperationException(
                    $"Unable to evaluate template language function '{functionName}': the second parameter is invalid. " +
                    $"Total width must be a positive integer value and not greater than '16', while '{totalLength}' was provided. " +
                    $"The syntax is {functionName}(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            // if specified, the padding character must be a single character.
            // if not specified, use a space as the default value
            if ((functionArgs.Length > 2) &&
                (functionArgs[2] is not string))
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function '{functionName}': the third parameter is invalid. " +
                    $"The padding character must be a string type, while '{ArgHelper.GetCapitalizedTypeName(functionArgs[2].GetType())}' was provided. " +
                    $"The syntax is {functionName}(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }
            var paddingString = (functionArgs.Length > 2)
                ? (string)functionArgs[2]
                : " ";
            if ((paddingString != null) && (paddingString.Length != 1))
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function '{functionName}': the third parameter is invalid. " +
                    $"The padding character must be a single character, while '{paddingString}' was provided. " +
                    $"The syntax is {functionName}(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }
            var paddingCharacter = paddingString[0];

            return valueToPad switch
            {
                int =>
                    ArmFunctions.PadLeft(
                        (int)valueToPad,
                        totalLength,
                        paddingCharacter
                    ),
                string =>
                    ArmFunctions.PadLeft(
                        (string)valueToPad,
                        totalLength,
                        paddingCharacter
                    ),
                _ =>
                    throw new InvalidOperationException()
            };

        }

        /// <summary>
        /// Returns a right-aligned string by adding characters to the left until reaching the total specified length.
        /// </summary>
        /// <returns>A string with at least the number of specified characters.</returns>
        /// <param name="valueToPad">The value to right-align.</param>
        /// <param name="totalLength">The total number of characters in the returned string.</param>
        /// <param name="paddingCharacter">The character to use for left-padding until the total length is reached. The default value is a space.</returns>
        /// <remarks>
        /// If the original string is longer than the number of characters to pad, no characters are added.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#padleft
        /// </remarks>
        /// <example>
        /// PadLeft("123", 10, '0') => "0000000123"
        /// </example>
        public static string PadLeft(int valueToPad, int totalLength, char paddingCharacter = ' ')
        {
            return ArmFunctions.PadLeft(
                valueToPad.ToString(), totalLength, paddingCharacter
            );
        }

        /// <summary>
        /// Returns a right-aligned string by adding characters to the left until reaching the total specified length.
        /// </summary>
        /// <returns>A string with at least the number of specified characters.</returns>
        /// <param name="valueToPad">The value to right-align.</param>
        /// <param name="totalLength">The total number of characters in the returned string.</param>
        /// <param name="paddingCharacter">The character to use for left-padding until the total length is reached. The default value is a space.</returns>
        /// <remarks>
        /// If the original string is longer than the number of characters to pad, no characters are added.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#padleft
        /// </remarks>
        /// <example>
        /// PadLeft("123", 10, '0') => "0000000123"
        /// </example>
        public static string PadLeft(string valueToPad, int totalLength, char paddingCharacter = ' ')
        {
            if (valueToPad == null)
            {
                throw new ArgumentNullException(nameof(valueToPad));
            }
            if (totalLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalLength));
            }
            return valueToPad.PadLeft(totalLength, paddingCharacter);
        }

        #endregion

    }

}
