using System;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Format


        public static object FormatBinder(object[] functionArgs)
        {

            const string functionName = "format";

            // throw for incorrect number of arguments
            // e.g. "format()", "format('one')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsAtLeastOne(
                functionName, functionArgs,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for invalid argument types
            var allowedArgTypes = new Type[] {
                typeof(string), typeof(int), typeof(long)
            };
            if (
                (functionArgs[0] is not string) ||
                functionArgs.Skip(1).Any(
                    item => (item == null) || !allowedArgTypes.Contains(item.GetType())
                )
            )
            {
                throw new ArgumentException(
                    $"Unable to evaluate language function '{functionName}': " +
                    $"the first argument must be a string literal, and the type of other arguments must be one of 'Array, Boolean, Date, Float, Guid, Integer, Null, Object, String, TimeSpan, Undefined, Uri'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }

            // unbundle the function arguments
            var formatString = functionArgs[0];
            var args = functionArgs.Skip(1).ToArray();

            return ArmFunctions.Format(
                (string)formatString, args
            );

        }

        /// <summary>
        /// Creates a formatted string from input values.
        /// </summary>
        /// <returns>A string of the first character.</returns>
        /// <param name="formatString">The composite format string.</param>
        /// <param name="args">The values to include in the formatted string.</param>
        /// <remarks>
        /// Use this function to format a string in your template. It uses the same
        /// formatting options as the System.String.Format method in .NET.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#{functionName}
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

    }

}
