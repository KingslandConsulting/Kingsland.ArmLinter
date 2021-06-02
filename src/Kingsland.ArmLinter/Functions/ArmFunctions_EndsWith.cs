using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region EndsWith

        public static bool EndsWithBinder(object[] functionArgs)
        {

            const string functionName = "endsWith";

            // throw for incorrect number of arguments
            // e.g. "endsWith()", "endsWith('one', 'two', 'three')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 2,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // validate the argument types
            ArgHelper.AssertArgumentTypes_v4(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(string) },
                    new[] { typeof(string) }
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            // unbundle the function arguments
            var stringToSearch = functionArgs[0];
            var stringToTest = functionArgs[1];

            return ArmFunctions.EndsWith(
                (string)stringToSearch, (string)stringToTest
            );

        }

        public static bool EndsWith(string stringToSearch, string stringToFind)
        {
            if (stringToSearch == null)
            {
                throw new ArgumentNullException(nameof(stringToSearch));
            }
            if (stringToFind == null)
            {
                throw new ArgumentNullException(nameof(stringToFind));
            }
            return stringToSearch.EndsWith(stringToFind, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

    }

}
