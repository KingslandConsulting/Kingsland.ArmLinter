using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Provides some reflection-based methods  method
    /// </summary>
    internal static class BindingHelper
    {

        /// <summary>
        /// Finds the best matching method based on the arguments and invokes it.
        /// If no methods match or if multiple methods match, an exception is thrwn.
        /// </summary>
        public static object Invoke(List<MethodInfo> methodInfos, object[] args)
        {
            if (methodInfos == null)
            {
                throw new ArgumentNullException(nameof(methodInfos));
            }
            var matches = methodInfos.Select(
                    m => (
                        MethodInfo: m,
                        Args: BindingHelper.TryBindParameters(m, args, out var argsOut) ?
                            argsOut : null
                    )
                ).Where(x => x.Args != null)
                .ToList();
            if (matches.Count == 0)
            {
                var message = "No method overloads match the arguments.\r\n" +
                    "\r\n" +
                    "Overloads are:\r\n" +
                    string.Join("\r\n", methodInfos.Select(m => m.ToString())) + "\r\n" +
                    "\r\n" +
                    "Arguments are:\r\n" +
                    string.Join("\r\n", args.Select(a => a.GetType().ToString()));
                throw new ArgumentException(message, nameof(methodInfos));
            }
            if (matches.Count > 1)
            {
                var message = "More than one method overload matches the arguments.\r\n" +
                    "\r\n" +
                    "Overloads are:\r\n" +
                    string.Join("\r\n", methodInfos.Select(m => m.ToString())) + "\r\n" +
                    "\r\n" +
                    "Arguments are:\r\n" +
                    string.Join("\r\n", args.Select(a => a.GetType().ToString()));
                throw new ArgumentException(message, nameof(methodInfos));
            }
            return matches[0].MethodInfo.Invoke(null, matches[0].Args);
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
        /// type signature of the method and returns true if they can, otherwise returns false.
        ///
        /// If the arguments can be modified to match the type signature of the method then the argsOut
        /// parameter will contain the modified arguments. If the arguments do not need adapting then
        /// argsOut will contain a copy of the original argsIn.
        ///
        /// Modifications that are applied are, if appropriate are:
        ///
        /// + Adding the default values for parameters where an argument is missing
        /// + Casting some types where a cast makes sense
        /// + Converting multiple "params" argument values into a single array-valued argument
        ///
        /// </returns>
        private static bool TryBindParameters(MethodInfo methodInfo, object[] argsIn, out object[] argsOut)
        {

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var parameters = methodInfo.GetParameters();
            var argsTmp = new List<object>(argsIn);

            var parameterIndex = 0;
            while (parameterIndex < parameters.Length)
            {

                var parameter = parameters[parameterIndex];

                // if there's no argument for this parameter we can
                // see if it has a default value and use that
                if (argsTmp.Count <= parameterIndex)
                {
                    if (!parameter.HasDefaultValue)
                    {
                        argsOut = null;
                        return false;
                    }
                    argsTmp.Add(parameter.DefaultValue);
                }

                // cast any parameters that don't quite match the required type
                if (parameter.ParameterType == typeof(char))
                {
                    if ((argsTmp[parameterIndex] is string castArg) && (castArg.Length == 1))
                    {
                        // ARM templates don't support a char type - everything is a string.
                        // so, if a parameter is a char, and the argument is a string consisting
                        // of a single char then we'llconvert it to a char
                        argsTmp[parameterIndex] = castArg[0];
                    }
                }

                // roll any "params" arguments up into a single array-valued argument
                var paramsAttribute = parameter.GetCustomAttribute(typeof(ParamArrayAttribute));
                if (paramsAttribute != null)
                {
                    if (parameterIndex != (parameters.Length - 1))
                    {
                        throw new InvalidOperationException("params must be the last parameter.");
                    }
                    // get the params args and check their types are ok
                    var elementType = parameter.ParameterType.GetElementType();
                    var paramsArgs = argsTmp.Skip(parameterIndex).ToArray();
                    if (!paramsArgs.All(a => a.GetType() == elementType))
                    {
                        argsOut = null;
                        return false;
                    }
                    // build the params array
                    var paramsArray = Array.CreateInstance(elementType, argsIn.Length - parameterIndex);
                    paramsArgs.CopyTo(paramsArray, 0);
                    // replace the params args
                    argsTmp = argsTmp.Take(parameterIndex).ToList();
                    argsTmp.Add(paramsArray);
                }

                // check the type of the argument matches the type of the parameter
                if (argsTmp[parameterIndex].GetType() != parameter.ParameterType)
                {
                    argsOut = null;
                    return false;
                }

                parameterIndex++;

            }

            // everything look ok
            argsOut = argsTmp.ToArray();
            return true;

        }

    }

}
