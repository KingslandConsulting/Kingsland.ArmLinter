using System;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Base64ToString

        public static string Base64ToStringBinder(object[] functionArgs)
        {

            const string functionName = "base64ToString";

            // throw for incorrect number of arguments
            // e.g. "base64ToString()", "base64ToString('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v2(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            // unbundle the function arguments
            var base64Value = functionArgs[0];

            return ArmFunctions.Base64ToString((string)base64Value);

        }

        public static string Base64ToString(string base64Value)
        {
            if (base64Value == null)
            {
                throw new ArgumentNullException(nameof(base64Value));
            }
            return Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    base64Value
                )
            );
        }

        #endregion

    }

}
