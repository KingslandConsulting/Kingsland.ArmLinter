using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Substring

        public static string SubstringBinder(object[] functionArgs)
        {

            const string functionName = "substring";

            // throw for incorrect number of arguments
            // e.g. "substring()", "substring('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if ((functionArgs.Length < 2) || (functionArgs.Length > 3))
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects at least two parameters and up to three parameters: " +
                    $"a string, a start index and optionally a length. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }

            // validate the argument types
            if (
                (functionArgs[0] is not string) ||
                ((functionArgs.Length > 1) && (functionArgs[1] is not int)) ||
                ((functionArgs.Length > 2) && (functionArgs[2] is not int))
            )
            {
                throw new ArgumentException();
            }

            // unbundle the function arguments
            return functionArgs.Length switch
            {
                2 => ArmFunctions.Substring(
                    (string)functionArgs[0],
                    (int)functionArgs[1]
                ),
                3 => ArmFunctions.Substring(
                    (string)functionArgs[0],
                    (int)functionArgs[1],
                    (int)functionArgs[2]
                ),
                _ => throw new ArgumentException()
            };

        }

        /// <summary>
        /// Returns a substring that starts at the specified character position and contains the specified number of characters.
        /// </summary>
        /// <returns>The substring. Or, an empty string if the length is zero.</returns>
        /// <param name="stringToParse">The original string from which the substring is extracted.</param>
        /// <param name="startIndex">The zero-based starting character position for the substring.</param>
        /// <param name="length">The number of characters for the substring. Must refer to a location within the string. Must be zero or greater.</param>
        /// <remarks>
        /// The function fails when the substring extends beyond the end of the string, or when
        /// length is less than zero. The following example fails with the error "The index and
        /// length parameters must refer to a location within the string. The index parameter:
        /// '0', the length parameter: '11', the length of the string parameter: '10'.".
        ///
        /// Substring("1234567890", 0, 11)
        ///
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#substring
        /// </remarks>
        /// <example>
        /// Substring("one two three", 4, 3) => "two"
        /// </example>
        public static string Substring(string stringToParse, int startIndex, int length)
        {
            var functionName = "substring";
            if (stringToParse == null)
            {
                throw new ArgumentNullException(nameof(stringToParse));
            }
            if (startIndex < 0)
            {
                throw new ArgumentException(
                    $"Unable to evaluate the template language function '{functionName}'. " +
                    $"The index parameter cannot be less than zero. " +
                    $"The index: '{startIndex}'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }
            if ((startIndex > 0) && (startIndex >= stringToParse.Length))
            {
                throw new ArgumentException(
                    $"Unable to evaluate the template language function '{functionName}'. " +
                    $"The index parameter cannot be larger than the length of the string. " +
                    $"The index parameter: '{startIndex}', the length of the string parameter: '{stringToParse.Length}'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }
            if (length < 0)
            {
                throw new ArgumentException(
                    $"Unable to evaluate the template language function '{functionName}'. " +
                    $"The length parameter cannot be less than zero. " +
                    $"The length parameter: '{length}'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }
            if (startIndex + length > stringToParse.Length)
            {
                throw new ArgumentException(
                    $"Unable to evaluate the template language function '{functionName}'. " +
                    $"The index and length parameters must refer to a location within the string. " +
                    $"The index parameter: '{startIndex}', the length parameter: '{length}', the length of the string parameter: '{stringToParse.Length}'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }
            return stringToParse.Substring(startIndex, length);
        }

        public static string Substring(string stringToParse, int startIndex)
        {
            return ArmFunctions.Substring(
                stringToParse,
                startIndex,
                (stringToParse?.Length ?? 0) - startIndex
            );
        }

        #endregion

    }

}
