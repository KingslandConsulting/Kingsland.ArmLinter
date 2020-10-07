﻿using System;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template Array functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array
    /// </summary>
    public static class ArmArrayFunctions
    {

        #region Concat Methods

        /// <summary>
        /// Combines multiple arrays and returns the concatenated array.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>An array of concatenated values.</returns>
        /// <remarks>
        /// This function can take any number of arguments, and can accept arrays for the parameters.
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-array#concat
        /// </remarks>
        /// <example>
        /// Concat(
        ///     new object[] { "1-1", "1-2", "1-3" },
        ///     new object[] { "2-1", "2-2", "2-3" }
        /// ) =>
        ///     new object[] { "1-1", "1-2", "1-3", "2-1", "2-2", "2-3" }
        /// </example>
        public static object[] Concat(params object[][] args)
        {
            return args switch
            {
                null =>
                    throw new ArgumentNullException(nameof(args)),
                object[][] { Length: 0 } =>
                    throw new ArgumentException($"{nameof(Concat)} requires at least one parameter."),
                _ =>
                    args.SelectMany(arg => arg).ToArray()
            };
        }

        #endregion

    }

}