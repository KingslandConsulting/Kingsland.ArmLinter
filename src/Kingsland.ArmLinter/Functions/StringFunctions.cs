using System;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template String functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public sealed class StringFunctions
    {

        #region Constructors

        internal StringFunctions()
        {
        }

        #endregion

        #region Base64 Methods

        /// <summary>
        /// Returns the base64 representation of the input string.
        /// </summary>
        /// <param name="inputString">The value to return as a base64 representation.</param>
        /// <returns>A string containing the base64 representation.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#base64
        /// </remarks>
        /// <example>
        /// Base64("one, two, three") => "b25lLCB0d28sIHRocmVl"
        /// </example>
        /// <example>
        /// Base64("{'one': 'a', 'two': 'b'}") => ???
        /// </example>
        public string Base64(string inputString)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes(inputString)
            );
        }

        #endregion

        #region Base64ToString Methods

        /// <summary>
        /// Converts a base64 representation to a string.
        /// </summary>
        /// <param name="base64Value">The base64 representation to convert to a string.</param>
        /// <returns>A string of the converted base64 value.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#base64tostring
        /// </remarks>
        /// <example>
        /// Base64ToString("b25lLCB0d28sIHRocmVl") => "one, two, three"
        /// </example>
        /// <example>
        /// Base64ToString(???) => "{'one': 'a', 'two': 'b'}"
        /// </example>
        public string Base64ToString(string base64Value)
        {
            if (base64Value == null)
            {
                throw new ArgumentNullException(nameof(base64Value));
            }
            return Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    base64Value
                )
            );
        }

        #endregion

        #region Concat Methods

        /// <summary>
        /// Combines multiple string values and returns the concatenated string.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>A string of concatenated values.</returns>
        /// <remarks>
        /// This function can take any number of arguments, and can accept strings for the parameters.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#concat
        /// </remarks>
        /// <example>
        /// Concat("prefix", "-", "suffix") => "prefix-suffix"
        /// </example>
        public string Concat(params string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            if (args.Length == 0)
            {
                throw new ArgumentException($"{nameof(Concat)} requires at least one parameter.");
            }
            return string.Join(string.Empty, args);
        }

        #endregion

        #region PadLeft Methods

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
        public string PadLeft(string valueToPad, long totalLength, char paddingCharacter = ' ')
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

        #region ToLower Methods

        /// <summary>
        /// Converts the specified string to lower case.
        /// </summary>
        /// <param name="stringToChange">The value to convert to lower case.</param>
        /// <returns>The string converted to lower case.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#tolower
        /// </remarks>
        /// <example>
        /// ToLower("One Two Three") => "one two three"
        /// </example>
        public string ToLower(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToLowerInvariant();
        }

        #endregion

        #region ToUpper Methods

        /// <summary>
        /// Converts the specified string to upper case.
        /// </summary>
        /// <param name="stringToChange">The value to convert to upper case.</param>
        /// <returns>The string converted to upper case.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#toupper
        /// </remarks>
        /// <example>
        /// ToUpper("One Two Three") => "ONE TWO THREE"
        /// </example>
        public string ToUpper(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToUpperInvariant();
        }

        #endregion

    }

}
