using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region DataUriToString


        public static object DataUriToStringBinder(object[] functionArgs)
        {

            const string functionName = "dataUriToString";

            // throw for incorrect number of arguments
            // e.g. "dataUriToString()", "dataUriToString('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // unbundle the function arguments
            var dataUriToConvert = functionArgs[0];

            // validate the argument types
            ArgHelper.AssertArgumentTypes_v2(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            return ArmFunctions.DataUriToString(
                (string)dataUriToConvert
            );

        }

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
                        throw new NotSupportedException(
                            $"The provided charset '{encodingName}' is not supported."
                        ),
                    "UTF-8" => Encoding.UTF8,
                    _ =>
                        throw new NotSupportedException(
                            $"The provided charset '{encodingName}' is not supported."
                        )
                };
            }

            var stringValue = encoding.GetString(dataUri.Data);
            var mediaType = string.IsNullOrEmpty(dataUri.MediaType) ? "text/plain" : dataUri.MediaType;

            return mediaType switch
            {
                "text/plain" =>
                    stringValue,
                "application/json" =>
                    // note - json deserialization returns an int or long
                    // for some data, but the arm api seems to re-serialize
                    // these as strings, so we'll preserve that behaviour
                    JsonConvert.DeserializeObject(stringValue) switch
                    {
                        int i => i.ToString(CultureInfo.InvariantCulture),
                        long l => l.ToString(CultureInfo.InvariantCulture),
                        object o => o
                    },
                _ =>
                    throw new InvalidOperationException($"unsupported media type '{dataUri.MediaType}'"),
            };

        }

        #endregion

    }

}
