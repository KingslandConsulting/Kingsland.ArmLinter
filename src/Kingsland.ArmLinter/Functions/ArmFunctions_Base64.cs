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

        #region Base64

        public static string Base64Binder(object[] functionArgs)
        {

            const string functionName = "base64";

            // throw for incorrect number of arguments
            // e.g. "base64()", "base64('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactlyOne_v2(
                functionName, functionArgs,
                $"https://aka.ms/arm-template-expressions"
            );

            // throw for invalid argument types
            ArgHelper.AssertArgumentTypes_v1(
                functionName, functionArgs,
                new Type[][] {
                    new [] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions"
            );

            // unbundle the function arguments
            var inputString = functionArgs[0];

            return ArmFunctions.Base64((string)inputString);

        }

        public static string Base64(string inputString)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes(inputString)
            );
        }

        #endregion

    }

}
