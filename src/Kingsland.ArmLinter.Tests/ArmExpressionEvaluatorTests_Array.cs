using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        public static class ArrayTests
        {

            public static class CreateArrayTests
            {

                [Test]
                public static void InvokingWithNoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[createArray()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment : 09:49:49 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: Unable to evaluate template language function 'createArray'. At least one parameter should be provided. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "createArray()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'createArray'. At least one parameter should be provided."
                    );
                }

                [Test]
                public static void ShouldCreateArrayFromStringValues()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "createArray('a', 'b', 'c')",
                        new object[] { "a", "b", "c" }
                    );
                }

                [Test]
                public static void ShouldCreateArrayFromIntValues()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "createArray(1, 2, 3)",
                        new object[] { 1, 2, 3 }
                    );
                }

            }

            public static class EmptyTests
            {

                /// <summary>
                /// note - "[empty(createArray())]" throws an exception because createArray
                /// requires at lease one parameter. we *can* however, create an empty array
                /// by intersecting two arrays that hav an empty conjunction, hence
                /// "[empty(intersection(createArray('aaa'), createArray('bbb')))]"
                /// </summary>
                [Test]
                public static void EmptyArrayShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[empty(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       False
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty(intersection(createArray('aaa'), createArray('bbb')))",
                        true
                    );
                }

            }

        }

    }

}
