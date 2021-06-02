using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region Empty

        public static bool EmptyBinder(object[] functionArgs)
        {

            const string functionName = "empty";

            // throw for incorrect number of arguments
            // e.g. "empty()", "empty('one', 'two')"
            ArgHelper.AssertArgumentsNotNull(functionName, functionArgs);
            ArgHelper.AssertArgumentsExactCount_v1(
                functionName, functionArgs, 1,
                $"https://aka.ms/arm-template-expressions/#{functionName}"
            );

            // validate the argument types
            ArgHelper.AssertArgumentTypes_v3_2(
                functionName, functionArgs,
                new Type[][] {
                    new[] { typeof(object), typeof(object[]), typeof(string)}
                },
                $"https://aka.ms/arm-template-expressions#{functionName}"
            );

            // unbundle the function arguments
            var itemToTest = functionArgs[0];

            return itemToTest switch
            {
                string _ => ArmFunctions.Empty((string)itemToTest),
                object[] _ => ArmFunctions.Empty((object[])itemToTest),
                _ => throw new InvalidCastException(),
            };

        }

        public static bool Empty(string itemToTest)
        {
            if (itemToTest == null)
            {
                throw new ArgumentNullException(nameof(itemToTest));
            }
            return (itemToTest.Length == 0);
        }

        public static bool Empty(object[] itemToTest)
        {
            if (itemToTest == null)
            {
                throw new ArgumentNullException(nameof(itemToTest));
            }
            return (itemToTest.Length == 0);
        }

        #endregion

    }

}
