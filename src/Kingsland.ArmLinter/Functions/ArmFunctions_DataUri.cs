using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region DataUri

        public static object DataUriBinder(object[] functionArgs)
        {

            const string functionName = "dataUri";

            // throw for incorrect number of arguments
            // e.g. "dataUri()", "dataUri('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // the error message from the arm api is misleading -
            // it says parameters must be a string or integer, but it also
            // accepts arrays of strings and integers, so we can't use the
            // ArgHelper type validators as they couple the acceptable
            // type list to the error message.
            return functionArgs[0] switch
            {
                string valueToConvert =>
                    ArmFunctions.DataUri(valueToConvert),
                int valueToConvert =>
                    ArmFunctions.DataUri(valueToConvert),
                long valueToConvert =>
                    ArmFunctions.DataUri(valueToConvert),
                object valueToConvert when functionArgs[0].GetType().IsArray &&
                    ((Array)functionArgs[0]).Cast<object>().All(
                        item => (item is string) || (item is int) || (item is long)
                    ) =>
                    ArmFunctions.DataUri((Array)valueToConvert),
                _ =>
                    throw new ArgumentException(
                        $"The template function '{functionName}' expects its parameter to be string or integer. " +
                        $"The provided value is of type '{ArgHelper.GetLowerCaseTypeName(functionArgs[0].GetType())}'. " +
                        $"Please see https://aka.ms/arm-template-expressions#{functionName} for usage details."
                    )
            };

        }

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
        public static string DataUri(string valueToConvert)
        {
            if (valueToConvert == null)
            {
                throw new ArgumentNullException(nameof(valueToConvert));
            }
            // see https://en.wikipedia.org/wiki/Data_URI_scheme
            return new DataUri(
                "text/plain",
                new Dictionary<string, string> {
                    { "charset", "utf8" }
                },
                Encoding.UTF8.GetBytes(valueToConvert)
            ).ToString(true);
        }

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
        public static string DataUri(long valueToConvert)
        {
            return ArmFunctions.DataUri(
                (object)valueToConvert
            );
        }

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
        public static string DataUri(string[] valueToConvert)
        {
            return ArmFunctions.DataUri(
                (object)valueToConvert
            );
        }

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
        public static string DataUri(int[] valueToConvert)
        {
            return ArmFunctions.DataUri(
                (object)valueToConvert
            );
        }

        private static string DataUri(object valueToConvert)
        {
            // see https://en.wikipedia.org/wiki/Data_URI_scheme
            return new DataUri(
                "application/json",
                new Dictionary<string, string> {
                    { "charset", "utf8" }
                },
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(valueToConvert)
                )
            ).ToString(true);
        }

        #endregion

    }

}
