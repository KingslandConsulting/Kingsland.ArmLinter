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

        // example error message from the Azure Resource Manager:
        //
        // not enough arguments - e.g. "[base64()]"
        // too many arguments - e.g. "[base64('one', 'two')]"
        // wrong type *and* wrong number - e.g. "[base64(100, 100)]"
        //   New-AzResourceGroupDeployment: 21:32:16 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'no_parameters' is not valid: The template language function 'base64' must have only one parameter. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: e07fb1b6-ff5e-4eab-b194-1e9812cad025
        //
        // base64ToString gives a slightly different error with more detail - e.g. "base64ToString()":
        //   New-AzResourceGroupDeployment: 21:56:01 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'docs_microsoft_com_example_base64Output' is not valid: Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 2 were provided.Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: 8ccf15a8-434e-4fb8-931e-0c25ffb08988
        //
        // concat (with variable number of args) gives a different error again - e.g. "concat()":
        //   New-AzResourceGroupDeployment: 22:02:45 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'docs_microsoft_com_example_base64Output' is not valid: Unable to evaluate template language function 'concat'. At least one parameter should be provided.Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: 583fc4b5-e29f-4914-a143-fa523df00786
        //
        // correct number of arguments but wrong type - e.g. "[base64(100)]"
        //   New-AzResourceGroupDeployment: 21:47:06 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'docs_microsoft_com_example_base64Output' is not valid: The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: 473dd259-9f48-4dab-b438-db789cd566ee
        //
        // correct number of arguments but wrong type - e.g. "[base64(createArray(100, 100))]"
        //   New-AzResourceGroupDeployment: 21:49:50 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'docs_microsoft_com_example_base64Output' is not valid: The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Array'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: 604e8dd3-0ad0-4edc-b202-18d1ba7c8f14
        //
        // non-existent function - e.g. "[nonexistent()]"
        //   New-AzResourceGroupDeployment: 21:50:43 - The deployment 'arm_functions_string_base64' failed with error(s). Showing 1 out of 1 error(s).
        //   Status Message: The template output 'docs_microsoft_com_example_base64Output' is not valid: The template function 'nonexistent' is not valid.Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
        //   CorrelationId: 215d46ea-150e-410f-9996-5ce302b0ef81


        private static readonly Dictionary<string, MethodInfo[]> _functionBindings = new Dictionary<string, MethodInfo[]>
        {
            { "base64", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Base64))
            }},
            { "base64ToString", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Base64ToString))
            }},
            { "createArray", new [] {
                typeof(ArmArrayFunctions).GetMethod(nameof(ArmArrayFunctions.CreateArray))
            }},
            { "concat", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Concat)),
                typeof(ArmArrayFunctions).GetMethod(nameof(ArmArrayFunctions.Concat))
            }},
            { "dataUri", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.DataUri))
            }},
            { "dataUriToString", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.DataUriToString))
            }},
            { "empty", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Empty))
            }},
            { "endsWith", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.EndsWith))
            }},
            { "first", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.First))
            }},
            { "format", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Format))
            }},
            { "indexOf", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.IndexOf))
            }},
            { "last", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Last))
            }},
            { "length", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Length))
            }},
            { "lastIndexOf", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.LastIndexOf))
            }},
            { "padLeft", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.PadLeft))
            }},
            { "replace", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Replace))
            }},
            { "split", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Split), new Type[] { typeof(string), typeof(string) }),
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Split), new Type[] { typeof(string), typeof(string[]) })
            }},
            { "skip", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Skip))
            }},
            { "startsWith", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.StartsWith))
            }},
            { "substring", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Substring))
            }},
            { "take", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Take))
            }},
            { "trim", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.Trim))
            }},
            { "toLower", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.ToLower))
            }},
            { "toUpper", new [] {
                typeof(ArmStringFunctions).GetMethod(nameof(ArmStringFunctions.ToUpper))
            }}
        };

        /// <summary>
        /// Finds the best matching method based on the arguments and invokes it.
        /// If no methods match or if multiple methods match, an exception is thrwn.
        /// </summary>
        public static object InvokeFunction(string name, object[] args)
        {
            if (!_functionBindings.ContainsKey(name))
            {
                throw new NotImplementedException($"The ARM Template function '{name}' is not implemented.");
            }
            var matches = BindingHelper.BindMethodInfos(_functionBindings[name], args).ToList();
            if (matches.Count == 0)
            {
                var message = "No method overloads match the arguments.\r\n" +
                    "\r\n" +
                    "Arguments are:\r\n" +
                    string.Join("\r\n", args.Select(arg => arg.GetType().ToString()));
                throw new InvalidOperationException(message);
            }
            if (matches.Count > 1)
            {
                var message = "More than one method overload matches the arguments.\r\n" +
                    "\r\n" +
                    "Overloads are:\r\n" +
                    string.Join("\r\n", _functionBindings[name].Select(m => m.ToString())) + "\r\n" +
                    "\r\n" +
                    "Arguments are:\r\n" +
                    string.Join("\r\n", args.Select(arg => arg.GetType().ToString()));
                throw new InvalidOperationException(message);
            }
            return matches[0].MethodInfo.Invoke(null, matches[0].Args);
        }

        internal static IEnumerable<(MethodInfo MethodInfo, object[] Args)> BindMethodInfos(IEnumerable<MethodInfo> methodInfos, object[] args)
        {
            return methodInfos
                .Select(
                    m => (
                        MethodInfo: m,
                        Args: BindingHelper.TryBindParameters(m, args, out var argsOut) ?
                            argsOut : null
                    )
                ).Where(x => x.Args != null);
        }

        /// <summary>
        /// Tries to prepare a list of arguments so that they can be used as parameters to
        /// invoke a MethodInfo with. Returns true if the arguments can be used with the
        /// MethodInfo, otherwise returns false.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="argsIn"></param>
        /// <param name="argsOut"></param>
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
        internal static bool TryBindParameters(MethodInfo methodInfo, object[] argsIn, out object[] argsOut)
        {

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var parameters = methodInfo.GetParameters();
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
                    if(
                        !BindingHelper.TryConvertArray(
                            argsIn.Skip(parameterIndex).ToArray(),
                            parameter.ParameterType.GetElementType(),
                            out var convertedArray
                        )
                    )
                    {
                        argsOut = null;
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
                return false;
            }

            // everything look ok
            argsOut = newArgs.ToArray();
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
        internal static bool TryConvertArray(Array originalArray, Type elementType, out object convertedArray)
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

    }

}
