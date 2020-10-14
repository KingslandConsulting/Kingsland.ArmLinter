using System;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template Array functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array
    /// </summary>
    public static class ArmArrayFunctions
    {

        #region Concat

        //public static object Concat(params object[][] args)
        //{
        //    return ArmStringFunctions.Concat(args);
        //}

        #endregion

        #region CreateArray

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
            if (args.Length < 1)
            {
                throw new ArgumentException("At least one parameter should be provided.");
            }
            return args;
        }

        #endregion

    }

}
