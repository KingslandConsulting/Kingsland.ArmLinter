using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingsland.ArmLinter.Functions
{

    internal static class ArgHelper
    {

        #region AssertArgumentsNotNull

        public static void AssertArgumentsNotNull(string functionName, object[] functionArgs)
        {
            if (functionArgs == null)
            {
                throw new ArgumentNullException(nameof(functionArgs));
            }
        }

        #endregion

        #region AssertArgumentsExactlyOne

        public static void AssertArgumentsExactlyOne_v1(string functionName, object[] functionArgs)
        {
            if (functionArgs.Length != 1)
            {
                throw new ArgumentException(
                    $"The template function '{functionName}' must have only one parameter."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'base64' must have only one parameter.
        /// Please see https://aka.ms/arm-template-expressions for usage details.
        /// </remarks>
        public static void AssertArgumentsExactlyOne_v2(
            string functionName, object[] functionArgs,
            string helpUri
        )
        {
            if (functionArgs.Length != 1)
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' must have only one parameter. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        #endregion

        #region AssertArgumentsExactCount

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="expectedLength"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// Unable to evaluate template language function 'first': function requires 1 argument(s) while 2 were provided.
        /// Please see https://aka.ms/arm-template-expressions/#first for usage details.
        /// </remarks>
        public static void AssertArgumentsExactCount_v1(
            string functionName, object[] functionArgs, int expectedLength,
            string helpUri
        )
        {
            if (functionArgs.Length != expectedLength)
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function '{functionName}': " +
                    $"function requires {expectedLength} argument(s) while {functionArgs.Length} were provided. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="expectedLength"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'replace' expects exactly '3' parameters.
        /// Please see https://aka.ms/arm-template-expressions for usage details.
        /// </remarks>
        public static void AssertArgumentsExactCount_v2(
            string functionName, object[] functionArgs, int expectedLength,
            string helpUri
        )
        {
            if (functionArgs.Length != expectedLength)
            {
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects exactly '{expectedLength}' parameters. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        #endregion

        #region AssertArgumentsAtLeastCount

        public static void AssertArgumentsAtLeastOne(
            string functionName, object[] functionArgs,
            string helpUri
        )
        {
            if (functionArgs.Length < 1)
            {
                throw new ArgumentException(
                    $"Unable to evaluate template language function '{functionName}'. " +
                    $"At least one parameter should be provided. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        #endregion

        #region AssertArgumentTypes

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'toUpper' expects its parameter to be of type 'String'.
        /// The provided value is of type 'Integer'.
        /// Please see https://aka.ms/arm-template-expressions for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v1(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameters = (allowedTypes.Length == 1) ? "parameter" : "parameters";
                var allowedTypeNames = string.Join(
                    " and ",
                    allowedTypes[argIndex].Select(
                        ArgHelper.GetCapitalizedTypeName
                    )
                );
                var providedTypeName = ArgHelper.GetCapitalizedTypeName(
                    functionArgs[argIndex].GetType()
                );
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its {parameters} to be of type '{allowedTypeNames}'. " +
                    $"The provided value is of type '{providedTypeName}'. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'base64ToString' expects its parameter to be a string.
        /// The provided value is of type 'Integer'.
        /// Please see https://aka.ms/arm-template-expressions#base64ToString for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v2(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameters = (allowedTypes.Length == 1) ? "parameter" : "parameters";
                var allowedTypeNames = string.Join(
                    " and ",
                    allowedTypes[argIndex].Select(
                        ArgHelper.GetLowerCaseTypeName
                    )
                );
                var providedTypeName = ArgHelper.GetCapitalizedTypeName(
                    functionArgs[argIndex].GetType()
                );
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its parameter to be a {allowedTypeNames}. " +
                    $"The provided value is of type '{providedTypeName}'. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'first' expects its parameter be an array or a string.
        /// The provided value is of type 'Integer'.
        /// Please see https://aka.ms/arm-template-expressions#first for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v3_1(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameters = (allowedTypes.Length == 1) ? "parameter" : "parameters";
                var allowedTypeParts = allowedTypes[argIndex].Select(
                    type =>
                        type switch
                        {
                            Type t when t == typeof(object) =>
                                "an object",
                            Type t when t == typeof(string) =>
                                "a string",
                            Type t when t == typeof(int) =>
                                "an integer",
                            Type t when t.IsArray =>
                                "an array",
                            _ => throw new ArgumentException(null, nameof(type))
                        }
                ).ToArray();
                var allowedTypeNames = (allowedTypeParts.Length == 1)
                    ? allowedTypeParts[0]
                    : string.Join(", ", allowedTypeParts[0..^1]) +
                      " or " + allowedTypeParts[^1];
                var providedTypeName = GetCapitalizedTypeName(
                    functionArgs[argIndex].GetType()
                );
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its {parameters} be {allowedTypeNames}. " +
                    $"The provided value is of type '{providedTypeName}'. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <remarks>
        /// Example:
        /// The template function 'empty' expects its parameter to be an object, an array, or a string.
        /// The provided value is of type 'Integer'.
        /// Please see https://aka.ms/arm-template-expressions#empty for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v3_2(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameters = (allowedTypes.Length == 1) ? "parameter" : "parameters";
                var allowedTypeParts = allowedTypes[argIndex].Select(
                    type =>
                        type switch
                        {
                            Type t when t == typeof(object) =>
                                "an object",
                            Type t when t == typeof(string) =>
                                "a string",
                            Type t when t == typeof(int) =>
                                "an integer",
                            Type t when t == typeof(long) =>
                                "a long",
                            Type t when t.IsArray =>
                                "an array",
                            _ => throw new ArgumentException(null, nameof(type))
                        }
                ).ToArray();
                var allowedTypeNames = (allowedTypeParts.Length == 1)
                    ? allowedTypeParts[0]
                    : string.Join(", ", allowedTypeParts[0..^1]) +
                      ", or " + allowedTypeParts[^1];
                var providedTypeName = GetCapitalizedTypeName(
                    functionArgs[argIndex].GetType()
                );
                throw new ArgumentException(
                    $"The template function '{functionName}' expects its {parameters} to be {allowedTypeNames}. " +
                    $"The provided value is of type '{providedTypeName}'. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'endsWith' expects its parameters to be of type string and string.
        /// The provided value is of type 'Array' and 'Array'.
        /// Please see https://aka.ms/arm-template-expressions#endsWith for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v4(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameters = (allowedTypes.Length == 1) ? "parameter" : "parameters";
                var allowedTypeNames = string.Join(
                    " and ",
                    allowedTypes.Select(
                        i => ArgHelper.GetLowerCaseTypeName(i[0])
                    )
                );
                var providedTypeNames = string.Join(" and ",
                    functionArgs.Select(
                        arg => $"'{ArgHelper.GetCapitalizedTypeName(arg.GetType())}'"
                    )
                );
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its {parameters} to be of type {allowedTypeNames}. " +
                    $"The provided value is of type {providedTypeNames}. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionArgs"></param>
        /// <param name="allowedTypes"></param>
        /// <param name="helpUri"></param>
        /// <remarks>
        /// Example:
        /// The template language function 'replace' expects its third parameter to be of type 'String'.
        /// The provided value is of type 'Array'.
        /// Please see https://aka.ms/arm-template-expressions for usage details.
        /// </remarks>
        public static void AssertArgumentTypes_v5(
            string functionName, object[] functionArgs, Type[][] allowedTypes,
            string helpUri
        )
        {
            var mismatches = ArgHelper.GetArgumentTypeMismatches(
                allowedTypes, functionArgs
            ).ToList();
            if (mismatches.Any())
            {
                var argIndex = mismatches[0];
                var parameterOrdinal = new[] {
                    "first", "second", "third", "fourth", "fifth"
                }[argIndex];
                var allowedTypeNames = string.Join(
                    " or ",
                    allowedTypes[argIndex].Select(
                        type => ArgHelper.GetCapitalizedTypeName(type)
                    )
                );
                var providedTypeName = ArgHelper.GetCapitalizedTypeName(
                    functionArgs[argIndex].GetType()
                );
                throw new ArgumentException(
                    $"The template language function '{functionName}' expects its {parameterOrdinal} parameter to be of type '{allowedTypeNames}'. " +
                    $"The provided value is of type '{providedTypeName}'. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        private static IEnumerable<int> GetArgumentTypeMismatches(
            Type[][] allowedTypes, object[] providedArgs
        )
        {
            if (allowedTypes.Length != providedArgs.Length)
            {
                throw new ArgumentException(null, nameof(allowedTypes));
            }
            for (var i = 0; i < providedArgs.Length; i++)
            {
                if (providedArgs[i] == null)
                {
                    throw new NullReferenceException();
                }
                if (!allowedTypes[i].Contains(providedArgs[i].GetType()))
                {
                    yield return i;
                }
            }
        }

        public static string GetCapitalizedTypeName(Type type)
        {
            return type switch
            {
                Type t when t == typeof(object) =>
                    "Object",
                Type t when t == typeof(string) =>
                    "String",
                Type t when t == typeof(int) =>
                    "Integer",
                Type t when t == typeof(string[]) =>
                    "Array of Strings",
                Type t when t.IsArray =>
                    "Array",
                _ => throw new ArgumentException(null, nameof(type))
            };
        }

        public static string GetLowerCaseTypeName(Type type)
        {
            return type switch
            {
                Type t when t == typeof(object) =>
                    "object",
                Type t when t == typeof(string) =>
                    "string",
                Type t when t == typeof(int) =>
                    "integer",
                Type t when t.IsArray =>
                    "array",
                _ => throw new ArgumentException(null, nameof(type))
            };
        }

        #endregion

        #region AssertArgumentsAllArrayOrNoneArray

        public static void AssertArgumentsAllArrayOrNoneArray(
            string functionName, object[] functionArgs,
            string helpUri
        )
        {
            var arrayCount = functionArgs.Count(item => item.GetType().IsArray);
            if ((arrayCount > 0) && (arrayCount < functionArgs.Length))
            {
                throw new ArgumentException(
                    $"The provided parameters for language function '{functionName}' are invalid. " +
                    $"Either all or none of the parameters must be an array. " +
                    $"Please see {helpUri} for usage details."
                );
            }
        }

        #endregion

    }

}
