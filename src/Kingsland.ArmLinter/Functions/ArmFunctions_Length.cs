using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Length

        public static int LengthBinder(object[] functionArgs)
        {

            const string functionName = "length";

            // throw for incorrect number of arguments
            // e.g. "length()", "length('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            if (functionArgs.Length != 1)
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects exactly one parameter: an array, object, or a string the length of which is returned. " +
                    $"The function was invoked with '{functionArgs.Length}' parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }

            // throw for invalid argument types
            if ((functionArgs[0] is not Array) &&
                //(functionArgs[0] is not object) &&
                (functionArgs[0] is not string))
            {
                throw new ArgumentException(
                    $"The template language function 'length' expects its parameter to be an array, object, or a string. " +
                    $"The provided value is of type '{ArgHelper.GetCapitalizedTypeName(functionArgs[0].GetType())}'. " +
                    $"Please see https://aka.ms/arm-template-expressions/#{functionName} for usage details."
                );
            }

            // unbundle the function arguments
            var arg1 = functionArgs[0];

            switch (arg1)
            {
                case string str:
                    return ArmFunctions.Length(str);
                case object[] arr:
                    return ArmFunctions.Length(arr);
                default:
                    throw new InvalidOperationException();
            };

        }

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

        public static int Length(object[] arg1)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException(nameof(arg1));
            }
            return arg1.Length;
        }

        #endregion

    }

}
