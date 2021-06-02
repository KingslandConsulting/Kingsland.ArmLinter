using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array
    /// </summary>
    public static partial class ArmFunctions
    {

        #region CreateArray


        public static object[] CreateArrayBinder(object[] functionArgs)
        {

            const string functionName = "createArray";

            // throw for incorrect number of arguments
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);

            // unbundle the function arguments
            var args = functionArgs;

            return ArmFunctions.CreateArray(
                args
            );

        }

        /// <summary>
        /// Creates an array from the parameters.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>An array.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array#createarray
        /// </remarks>
        /// <example>
        /// CreateArray("a", "b", "c") =>
        ///     new object[] { "a", "b", "c" }
        /// </example>
        /// <example>
        /// CreateArray(1, 2, 3) =>
        ///     new object[] { 1, 2, 3 }
        /// </example>
        public static object[] CreateArray(params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            return args;
        }

        #endregion

    }

}
