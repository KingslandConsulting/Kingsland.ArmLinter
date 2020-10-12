using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template String functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static class ArmStringFunctions
    {

        #region Base64

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
        public static string Base64(string inputString)
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

        #region Base64ToString

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
        public static string Base64ToString(string base64Value)
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

        #region Concat

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
        public static string Concat(params string[] args)
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

        #region DataUri

        /// <summary>
        /// Converts a value to a data URI.
        /// </summary>
        /// <param name="valueToConvert">The value to convert to a data URI.</param>
        /// <returns>A string formatted as a data URI.</returns>
        /// <remarks>
        /// note - the documentation defines the parameter as "stringToConvert", but the deployment api supports
        /// integers as well - e.g. "dataUri(100)" => "data:application/json;charset=utf8;base64,MTAw"
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#datauri
        /// </remarks>
        /// <example>
        /// DataUri("Hello") => "data:text/plain;charset=utf8;base64,SGVsbG8="
        /// </example>
        public static string DataUri(object valueToConvert)
        {
            if (valueToConvert == null)
            {
                throw new ArgumentNullException(nameof(valueToConvert));
            }
            // see https://en.wikipedia.org/wiki/Data_URI_scheme
            switch (valueToConvert)
            {
                case string s:
                    return new DataUri(
                        "text/plain",
                        new Dictionary<string, string> { { "charset", "utf8" } },
                        Encoding.UTF8.GetBytes(s)
                    ).ToString(true);
                case int _:
                case long _:
                    return new DataUri(
                        "application/json",
                        new Dictionary<string, string> { { "charset", "utf8" } },
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(valueToConvert)
                        )
                    ).ToString(true);
                default:
                    throw new InvalidOperationException();
            };
        }

        #endregion

        #region DataUriToString

        /// <summary>
        /// Converts a data URI formatted value to a string.
        /// </summary>
        /// <param name="dataUriToConvert">The data URI value to convert.</param>
        /// <returns>A string containing the converted value.</returns>
        /// <remarks>
        /// note - the documentation defines the return type as "string", but the deployment api supports
        /// integers as well - e.g. "dataUri(100)" => "data:application/json;charset=utf8;base64,MTAw"
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#datauritostring
        /// </remarks>
        /// <example>
        /// DateUriToString("data:;base64,SGVsbG8sIFdvcmxkIQ==") => "Hello, World!"
        /// </example>
        public static object DataUriToString(string stringToConvert)
        {
            if (stringToConvert == null)
            {
                throw new ArgumentNullException(nameof(stringToConvert));
            }
            var dataUri = Functions.DataUri.Parse(stringToConvert);
            var encoding = Encoding.ASCII;
            if (dataUri.Parameters.TryGetValue("charset", out var encodingName))
            {
                encoding = encodingName switch
                {
                    "utf8" =>
                        // see https://github.com/Azure/azure-powershell/issues/13179
                        // the "dataUri" function generates strings with "charset=utf8", but
                        // "dataUriToString" doesn't recognize this encoding, so we'll
                        // replicate the behaviour for now
                        throw new NotSupportedException($"The provided charset '{encodingName}' is not supported."),
                    "UTF-8" => Encoding.UTF8,
                    _ =>
                        throw new NotSupportedException($"The provided charset '{encodingName}' is not supported.")
                };
            }
            var stringValue = encoding.GetString(dataUri.Data);
            var mediaType = string.IsNullOrEmpty(dataUri.MediaType) ? "text/plain" : dataUri.MediaType;
            switch (mediaType)
            {
                case "text/plain":
                    return stringValue;
                case "application/json":
                    return JsonConvert.DeserializeObject(stringValue);
                default:
                    throw new InvalidOperationException($"unsupported media type '{dataUri.MediaType}'");
            }
            return encoding.GetString(dataUri.Data);
        }

        #endregion

        #region Empty

        /// <summary>
        /// Determines if a string is empty.
        /// </summary>
        /// <returns>Returns True if the value is empty; otherwise, False.</returns>
        /// <param name="itemToTest">The value to check if it's empty.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#empty
        /// </remarks>
        /// <example>
        /// Empty("") => true
        /// </example>
        public static bool Empty(string itemToTest)
        {
            if (itemToTest == null)
            {
                throw new ArgumentNullException(nameof(itemToTest));
            }
            return (itemToTest.Length == 0);
        }

        #endregion

        #region EndsWith

        /// <summary>
        /// Determines whether a string ends with a value. The comparison is case-insensitive.
        /// </summary>
        /// <returns>True if the last character or characters of the string match the value; otherwise, False.</returns>
        /// <param name="stringToSearch">The value that contains the item to find.</param>
        /// <param name="stringToFind">The value to find.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#startswith
        /// </remarks>
        /// <example>
        /// EndsWith("abcdef", "ef") => true
        /// </example>
        /// <example>
        /// EndsWith("abcdef", "F") => true
        /// </example>
        /// <example>
        /// EndsWith("abcdef", "e") => false
        /// </example>
        public static bool EndsWith(string stringToSearch, string stringToFind)
        {
            if (stringToSearch == null)
            {
                throw new ArgumentNullException(nameof(stringToSearch));
            }
            if (stringToFind == null)
            {
                throw new ArgumentNullException(nameof(stringToFind));
            }
            return stringToSearch.EndsWith(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region First

        /// <summary>
        /// Returns the first character of the string.
        /// </summary>
        /// <returns>A string of the first character.</returns>
        /// <param name="arg1">The value to retrieve the first character.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#first
        /// </remarks>
        /// <example>
        /// First("One Two Three") => "O"
        /// </example>
        public static string First(string arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return (arg1.Length == 0) ?
                string.Empty :
                new string(arg1[0], 1);
        }

        #endregion

        #region Format

        /// <summary>
        /// Creates a formatted string from input values.
        /// </summary>
        /// <returns>A string of the first character.</returns>
        /// <param name="formatString">The composite format string.</param>
        /// <param name="args">The values to include in the formatted string.</param>
        /// <remarks>
        /// Use this function to format a string in your template. It uses the same
        /// formatting options as the System.String.Format method in .NET.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#format
        ///     https://docs.microsoft.com/en-us/dotnet/api/system.string.format?view=netcore-3.1
        /// </remarks>
        /// <example>
        /// Format(
        ///     "{0}, {1}. Formatted number: {2:N0}",
        ///     "Hello", "User", 8175133"
        /// =>
        /// "Hello, User. Formatted number: 8,175,133"
        /// </example>
        public static string Format(string formatString, params object[] args)
        {
            if (formatString == null)
            {
                throw new ArgumentNullException(nameof(formatString));
            }
            return string.Format(formatString, args);
        }

        #endregion

        #region IndexOf

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

        #region Last

        /// <summary>
        /// Returns last character of the string.
        /// </summary>
        /// <returns>A string of the first character.</returns>
        /// <param name="arg1">The value to retrieve the last character.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#last
        /// </remarks>
        /// <example>
        /// Last("One Two Three") => "e"
        /// </example>
        public static string Last(string arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return (arg1.Length == 0) ?
                string.Empty :
                new string(arg1[arg1.Length - 1], 1);
        }

        #endregion

        #region LastIndexOf

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
            return stringToSearch.LastIndexOf(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Length

        /// <summary>
        /// Returns the number of characters in a string.
        /// </summary>
        /// <returns>An int.</returns>
        /// <param name="arg1">The string to use for getting the number of characters.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#length
        /// </remarks>
        /// <example>
        /// Length("One Two Three") => 13
        /// </example>
        public static int Length(string arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return arg1.Length;
        }

        #endregion

        #region PadLeft

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

        #region Replace

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

        #region Skip

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

        #region Split

        /// <summary>
        /// Returns an array of strings that contains the substrings of the input string that are delimited by the specified delimiters.
        /// </summary>
        /// <returns>An array of strings.</returns>
        /// <param name="inputString">The string to split.</param>
        /// <param name="delimiter">The delimiter to use for splitting the string.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#split
        /// </remarks>
        /// <example>
        /// Split("one,two,three", ",") => new string[] { "one", "two", "three" }
        /// </example>
        public static string[] Split(string inputString, string delimiter)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            return inputString.Split(delimiter);
        }

        /// <summary>
        /// Returns an array of strings that contains the substrings of the input string that are delimited by the specified delimiters.
        /// </summary>
        /// <returns>An array of strings.</returns>
        /// <param name="inputString">The string to split.</param>
        /// <param name="delimiters">The delimiters to use for splitting the string.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#split
        /// </remarks>
        /// <example>
        /// Split("one,two,three", ",") => new string[] { "one", "two", "three" }
        /// </example>
        public static string[] Split(string inputString, string[] delimiters)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            return inputString.Split(delimiters, StringSplitOptions.None);
        }

        #endregion

        #region StartsWith

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

        #region Substring

        /// <summary>
        /// Returns a substring that starts at the specified character position and contains the specified number of characters.
        /// </summary>
        /// <returns>The substring. Or, an empty string if the length is zero.</returns>
        /// <param name="stringToParse">The original string from which the substring is extracted.</param>
        /// <param name="startIndex">The zero-based starting character position for the substring.</param>
        /// <param name="length">	The number of characters for the substring. Must refer to a location within the string. Must be zero or greater.</param>
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
            if (stringToParse == null)
            {
                throw new ArgumentNullException(nameof(stringToParse));
            }
            return stringToParse.Substring(startIndex, length);
        }

        #endregion

        #region Take

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

        #region ToLower

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
        public static string ToLower(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToLowerInvariant();
        }

        #endregion

        #region ToUpper

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
        public static string ToUpper(string stringToChange)
        {
            if (stringToChange == null)
            {
                throw new ArgumentNullException(nameof(stringToChange));
            }
            return stringToChange.ToUpperInvariant();
        }

        #endregion

        #region Trim

        /// <summary>
        /// Removes all leading and trailing white-space characters from the specified string.
        /// </summary>
        /// <returns>The string without leading and trailing white-space characters.</returns>
        /// <param name="stringToTrim">The value to trim.</param>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#trim
        /// </remarks>
        /// <example>
        /// Take("    one two three   ") => "one two three"
        /// </example>
        public static string Trim(string stringToTrim)
        {
            if (stringToTrim == null)
            {
                throw new ArgumentNullException(nameof(stringToTrim));
            }
            return stringToTrim.Trim();
        }

        #endregion

        #region UniqueString

        /// <summary>
        /// Creates a deterministic hash string based on the values provided as parameters.
        /// </summary>
        /// <returns>A string containing 13 characters.</returns>
        /// <param name="baseString">The value used in the hash function to create a unique string.</param>
        /// <param name="args">You can add as many strings as needed to create the value that specifies the level of uniqueness.</param>
        /// <remarks>
        /// This function is helpful when you need to create a unique name for a resource.
        /// You provide parameter values that limit the scope of uniqueness for the result.
        /// You can specify whether the name is unique down to subscription, resource group,
        /// or deployment.
        ///
        /// The returned value isn't a random string, but rather the result of a hash function.
        /// The returned value is 13 characters long. It isn't globally unique. You may want to
        /// combine the value with a prefix from your naming convention to create a name that
        /// is meaningful.The following example shows the format of the returned value. The
        /// actual value varies by the provided parameters.
        ///
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#uniquestring
        /// </remarks>
        public static string UniqueString(string baseString, params string[] args)
        {

            // applies a currently-unknown deterministic hash algorithm to turn the
            // arguments into a predictable 13-character string. the same input always
            // produces the same hash value.

            // the algorithm can be executed by deploying the following arm template:

            // {
            //   "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
            //   "contentVersion": "1.0.0.0",
            //   "resources" : [ ],
            //   "outputs": {
            //     "uniqueString": {
            //       "type": "string",
            //       "value": "[uniqueString('xxx'))]"
            //     }
            //   }
            // }

            // PS> New-AzResourceGroupDeployment -ResourceGroupName "my-resourcegroup" -TemplateFile "C:\temp\template.json"

            // example output:
            //
            //   uniqueString("aaa") => "zavj67f3en4hc"
            //   uniqueString("bbb") => "w5eragfxe33cy"
            //   uniqueString("zzz") => "cryksdzzzawae"
            //   uniqueString("xxx", "yyy", "zzz") => "lghi7k5js7itq"

            // the actual algorithm is not published anywhere, and there's seemingly no information
            // that will allow us to replicate it. here's some links to the limited information that
            // *is* available:
            //
            //   + https://stackoverflow.com/questions/43295720/azure-arm-uniquestring-function-mimic
            //   + https://stackoverflow.com/questions/64119824/reimplement-the-uniquestring-hash-arm-function
            //   + https://docs.microsoft.com/en-gb/archive/blogs/389thoughts/get-uniquestring-generate-unique-id-for-azure-deployments

            if (baseString == null)
            {
                throw new ArgumentNullException(nameof(baseString));
            }

            throw new NotImplementedException();

        }

        #endregion

    }

}
