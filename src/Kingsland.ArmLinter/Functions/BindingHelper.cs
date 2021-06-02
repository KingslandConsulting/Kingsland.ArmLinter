using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Provides some reflection-based methods to invoke ARM Template functions.
    /// </summary>
    internal static class BindingHelper
    {

        private static readonly Dictionary<string, MethodInfo> _functionBindings = new Dictionary<string, MethodInfo>
        {
            { "base64", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.Base64Binder)) },
            { "base64ToString", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.Base64ToStringBinder)) },
            { "concat", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.ConcatBinder)) },
            { "createArray", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.CreateArray)) },
            { "dataUri", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.DataUriBinder)) },
            { "dataUriToString", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.DataUriToStringBinder)) },
            { "empty", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.EmptyBinder)) },
            { "endsWith", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.EndsWithBinder)) },
            { "first", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.FirstBinder)) },
            { "format", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.FormatBinder)) },
            { "indexOf", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.IndexOfBinder)) },
            { "intersection", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.IntersectionBinder)) },
            { "last", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.LastBinder)) },
            { "length", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.LengthBinder)) },
            { "lastIndexOf", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.LastIndexOfBinder)) },
            { "padLeft", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.PadLeftBinder)) },
            { "replace", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.ReplaceBinder)) },
            { "skip", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.SkipBinder)) },
            { "split", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.SplitBinder)) },
            { "startsWith", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.StartsWithBinder)) },
            { "substring", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.SubstringBinder)) },
            { "take", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.TakeBinder)) },
            { "toLower", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.ToLowerBinder)) },
            { "toUpper", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.ToUpperBinder)) },
            { "trim", typeof(ArmFunctions).GetMethod(nameof(ArmFunctions.TrimBinder)) }
        };

        /// <summary>
        /// Finds the best matching method based on the arguments and invokes it.
        /// If no methods match or if multiple methods match, an exception is thrwn.
        /// </summary>
        public static object InvokeFunction(string name, object[] args)
        {
            if (!_functionBindings.ContainsKey(name))
            {
                throw new InvalidOperationException($"The template function '{name}' is not valid.");
            }
            var functionInfo = _functionBindings[name];
            //if (!BindingHelper.TryBindParameters(name, functionInfo, args, out var convertedArgs, out var errorMessage))
            //{
            //    throw new InvalidOperationException(errorMessage);
            //}
            try
            {
                return functionInfo.Invoke(null, new object[] { args });
            }
            catch (TargetInvocationException ex)
            {
                //throw new InvalidOperationException(
                //    $"Unable to evaluate template language function '{name}'. {ex.InnerException.Message}"
                //);
                //throw new InvalidOperationException(ex.InnerException.Message);
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Tries to prepare a list of arguments so that they can be used as parameters to
        /// invoke a MethodInfo with. Returns true if the arguments can be used with the
        /// MethodInfo, otherwise returns false.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="functionInfo"></param>
        /// <param name="argsIn"></param>
        /// <param name="argsOut"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        /// Given a MethodInfo and a list of arguments, this function checks if the arguments satisfy the
        /// type signature of the method and returns true if they do, otherwise returns false.
        ///
        /// If the arguments can be modified to match the type signature of the method then the argsOut
        /// parameter will contain the modified arguments. If the arguments do not need to be modified
        /// then argsOut will contain a copy of the original argsIn.
        ///
        /// Modifications that are applied if appropriate are:
        ///
        /// + Adding a parameter's default value (if it has one) where an argument is missing
        /// + Casting the value of arguments to match the type of a parameter where a cast makes sense
        /// + Converting multiple "params" argument values into a single array-valued argument
        /// + Converting array arguments where the element type needs to be different
        ///
        /// </returns>
        internal static bool TryBindParameters(string functionName, MethodInfo functionInfo, object[] argsIn, out object[] argsOut, out string errorMessage)
        {

            if (functionInfo == null)
            {
                throw new ArgumentNullException(nameof(functionInfo));
            }

            var parameters = functionInfo.GetParameters();
            var newArgs = new List<object>(argsIn);

            var parameterIndex = 0;
            while (parameterIndex < parameters.Length)
            {

                var parameter = parameters[parameterIndex];

                // check if this is a "params" parameter with a dynamic number of arguments
                var paramsAttribute = parameter.GetCustomAttribute(typeof(ParamArrayAttribute));
                if (paramsAttribute != null)
                {

                    if (parameterIndex != (parameters.Length - 1))
                    {
                        throw new InvalidOperationException("params must be the last parameter.");
                    }

                    // convert the params array
                    if (
                        !BindingHelper.TryConvertArray(
                            argsIn.Skip(parameterIndex).ToArray(),
                            parameter.ParameterType.GetElementType(),
                            out var convertedArray
                        )
                    )
                    {
                        argsOut = null;
                        errorMessage = "params error";
                        return false;
                    }

                    // replace the multiple "params" args with the params array
                    newArgs = newArgs.Take(parameterIndex).ToList();
                    newArgs.Add(convertedArray);

                }
                else if (newArgs.Count <= parameterIndex)
                {
                    // if there's no argument for this parameter we can
                    // see if it has a default value and use that
                    if (parameter.HasDefaultValue)
                    {
                        newArgs.Add(parameter.DefaultValue);
                    }
                    else
                    {
                        argsOut = null;
                        errorMessage = BindingHelper.GetArgumentCountError(
                            functionName, parameters.Length, argsIn.Length
                        );
                        return false;
                    }
                }
                else if (BindingHelper.TryConvertValue(newArgs[parameterIndex], parameter.ParameterType, out var convertedArg))
                {
                    // try to convert any parameters that don't quite match the required type
                    newArgs[parameterIndex] = convertedArg;
                }
                else
                {
                    // we couldn't convert the argument type to match the parameter type
                    argsOut = null;
                    errorMessage = BindingHelper.GetArgumentTypeMismatchErrorMessage(
                        functionName, functionInfo, argsIn
                    );
                    return false;
                }

                parameterIndex++;

            }

            // we've processed all the method parameters and arguments, but
            // do we have the same number of each now? (i.e. were there the
            // right number of arguments in the attempted call to the mathod?)
            if (newArgs.Count != parameters.Length)
            {
                argsOut = null;
                errorMessage = BindingHelper.GetArgumentCountError(
                    functionName, parameters.Length, argsIn.Length
                );
                return false;
            }

            // everything look ok
            argsOut = newArgs.ToArray();
            errorMessage = null;
            return true;

        }

        /// <summary>
        /// Attempts to convert the given value to a different type. Some of this
        /// conversion is specific to ARM Templates (e.g. string -> char), and
        /// some of it is a (very) limited version of what the C# compiler does.
        /// </summary>
        /// <param name="originalValue"></param>
        /// <param name="desiredType"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        internal static bool TryConvertValue(object originalValue, Type desiredType, out object convertedValue)
        {
            if (originalValue == null)
            {
                convertedValue = originalValue;
                return true;
            }
            if (desiredType.IsAssignableFrom(originalValue.GetType()))
            {
                // it's already the desired type, so no conversion necessary
                convertedValue = originalValue;
                return true;
            }
            if (desiredType == typeof(object))
            {
                // value types return false for IsAssignablFrom()
                // but we can still box them into objects
                convertedValue = originalValue;
                return true;
            }
            // ARM templates don't support a char type - everything is a string,
            // so if a parameter is a char and the argument is a string consisting
            // of a single char then we'll convert it to a char
            if (desiredType == typeof(char))
            {
                if ((originalValue is string stringArg) && (stringArg.Length == 1))
                {
                    convertedValue = stringArg[0];
                    return true;
                }
            }
            // can we cast arrays? e.g. object[] -> string[]
            // see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/
            if (originalValue.GetType().IsArray && desiredType.IsArray)
            {
                if (BindingHelper.TryConvertArray((Array)originalValue, desiredType.GetElementType(), out var convertedArray))
                {
                    convertedValue = convertedArray;
                    return true;
                }
            }
            convertedValue = null;
            return false;
        }

        public delegate bool TypeConverter<TIn, TOut>(TIn argIn, out TOut argOut);

        internal static bool TryConvertArray<TIn, TOut>(Array originalArray, TypeConverter<TIn, TOut> typeConverter, out TOut[] convertedArray)
        {
            // if the original array is the corret type, just return that
            if (originalArray is TOut[] castArray)
            {
                convertedArray = castArray;
                return true;
            }
            // otherwise, see if we can convert each element
            var tmpArray = (TOut[])Array.CreateInstance(typeof(TOut), originalArray.Length);
            for (var index = 0; index < originalArray.Length; index++)
            {
                if (typeConverter((TIn)originalArray.GetValue(index), out var convertedValue))
                {
                    tmpArray.SetValue(convertedValue, index);
                }
                else
                {
                    convertedArray = null;
                    return false;
                }
            }
            convertedArray = tmpArray;
            return true;
        }

        /// <summary>
        /// A poor-mans's contravariance / covariance implementation. Given an
        /// array of one type of element, TryConvertArray attempts to convert
        /// it into an array of another compatible type.
        ///
        /// For example, given a string[] we can convert it into an object[].
        /// And, given an object[] where all the items are strings, we can
        /// convert it into a string[].
        /// </summary>
        /// <param name="originalArray"></param>
        /// <param name="elementType"></param>
        /// <param name="convertedArray"></param>
        /// <returns></returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance#:~:text=Covariance%20and%20contravariance%20are%20terms,assigning%20and%20using%20generic%20types.
        /// </remarks>
        internal static bool TryConvertArray(Array originalArray, Type elementType, out Array convertedArray)
        {
            var tmpArray = Array.CreateInstance(elementType, originalArray.Length);
            for (var index = 0; index < originalArray.Length; index++)
            {
                if (BindingHelper.TryConvertValue(originalArray.GetValue(index), elementType, out var convertedValue))
                {
                    tmpArray.SetValue(convertedValue, index);
                }
                else
                {
                    convertedArray = null;
                    return false;
                }
            }
            convertedArray = tmpArray;
            return true;
        }

        /// <summary>
        /// A poor-mans's contravariance / covariance implementation. Given an
        /// array of one type of element, TryConvertArray attempts to convert
        /// it into an array of another compatible type.
        ///
        /// For example, given a string[] we can convert it into an object[].
        /// And, given an object[] where all the items are strings, we can
        /// convert it into a string[].
        /// </summary>
        /// <param name="originalArray"></param>
        /// <param name="elementType"></param>
        /// <param name="convertedArray"></param>
        /// <returns></returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance#:~:text=Covariance%20and%20contravariance%20are%20terms,assigning%20and%20using%20generic%20types.
        /// </remarks>
        internal static bool TryConvertArray<TOut>(Array originalArray, out TOut[] convertedArray)
        {
            var tmpArray = new TOut[originalArray.Length];
            for (var index = 0; index < originalArray.Length; index++)
            {
                if (BindingHelper.TryConvertValue(originalArray.GetValue(index), typeof(TOut), out var convertedValue))
                {
                    tmpArray.SetValue(convertedValue, index);
                }
                else
                {
                    convertedArray = null;
                    return false;
                }
            }
            convertedArray = tmpArray;
            return true;
        }

        private static string GetArgumentTypeMismatchErrorMessage(string functionName, MethodInfo functionInfo, object[] args)
        {
            static string MapTypeName(Type type)
            {
                return type switch
                {
                    Type t when t == typeof(string) =>
                        "String",
                    Type t when t == typeof(int) =>
                        "Integer",
                    Type t when t.IsArray =>
                        "Array",
                    _ => type.Name
                };
            }
            var parameters = functionInfo.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    throw new InvalidOperationException();
                case 1:
                    // there's two slightly different forms to this error message - e.g.:
                    //
                    //   + "The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Array'."
                    //   + "The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Integer'."
                    //
                    // "The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. The function was invoked with '2' parameters."
                    switch (functionName)
                    {
                        case "base64ToString":
                        case "dataUriToString":
                        case "first":
                            return
                                $"The template language function '{functionName}' expects its parameter to be a {parameters[0].ParameterType.Name.ToLowerInvariant()}. " +
                                $"The provided value is of type '{MapTypeName(args[0].GetType())}'.";
                        default:
                            return
                                $"The template language function '{functionName}' expects its parameter to be of type '{parameters[0].ParameterType.Name}'. " +
                                $"The provided value is of type '{MapTypeName(args[0].GetType())}'.";
                    }
                default:
                    // there's two slightly different forms to this error message - e.g.:
                    //
                    //   + "The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'."
                    switch (functionName)
                    {
                        case "endsWith":
                        case "indexOf":
                        case "lastIndexOf":
                            return
                                $"The template language function '{functionName}' expects its parameters to be of type " +
                                string.Join(" and ", parameters.Select(p => p.ParameterType.Name.ToLowerInvariant())) + ". " +
                                $"The provided value is of type " +
                                string.Join(" and ", args.Select(a => $"'{MapTypeName(a.GetType())}'")) + ".";
                        default:
                            return
                                $"The template language function '{functionName}' expects its parameters to be of type " +
                                string.Join(" and ", parameters.Select(p => MapTypeName(p.ParameterType))) + ". " +
                                $"The provided value is of type " +
                                string.Join(" and ", args.Select(a => $"'{MapTypeName(a.GetType())}'")) + ". ";
                    }
            };
        }

        private static string GetArgumentCountError(string functionName, int parameterCount, int argCount)
        {
            switch (functionName)
            {
                case "base64":
                    return $"The template language function '{functionName}' must have only one parameter.";
                default:
                    return
                        $"Unable to evaluate template language function '{functionName}': " +
                        $"function requires {parameterCount} argument(s) while {argCount} were provided.";
            };
        }

    }

}
