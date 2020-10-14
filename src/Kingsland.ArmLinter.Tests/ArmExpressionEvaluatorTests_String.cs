using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        public static class StringTests
        {

            public static class Base64Tests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:35:41 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64' must have only one parameter. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64()",
                        typeof(InvalidOperationException),
                        "The template language function 'base64' must have only one parameter."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:36:14 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64' must have only one parameter. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64('one', 'two')",
                        typeof(InvalidOperationException),
                        "The template language function 'base64' must have only one parameter."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:36:47 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void ArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64(createArray(''))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:37:15 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Array'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64(createArray(''))",
                        typeof(InvalidOperationException),
                        "The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Array'."
                    );
                }

                [Test]
                public static void SampleStringShouldEncodeToBase64()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64('one, two, three')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     b25lLCB0d28sIHRocmVl
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64('one, two, three')",
                        "b25lLCB0d28sIHRocmVl"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString(base64('one, two, three'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     one, two, three
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64ToString(base64('one, two, three'))",
                        "one, two, three"
                    );
                }

            }

            public static class Base64ToStringTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:38:23 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:38:52 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:39:20 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void ArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString(createArray(''))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 12:52:56 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Array'. Please see https://aka.ms/arm-template-expressions#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString(createArray(''))",
                        typeof(InvalidOperationException),
                        "The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Array'."
                    );
                }

                [Test]
                public static void SampleBase64StringShouldDecodeToOriginalString()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64ToString('b25lLCB0d28sIHRocmVl')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     one, two, three
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64ToString('b25lLCB0d28sIHRocmVl')",
                        "one, two, three"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[base64(base64ToString('b25lLCB0d28sIHRocmVl'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     b25lLCB0d28sIHRocmVl
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64(base64ToString('b25lLCB0d28sIHRocmVl'))",
                        "b25lLCB0d28sIHRocmVl"
                    );
                }

            }

            public static class ConcatTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 10:02:51 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'concat'. At least one parameter should be provided. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "concat()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'concat'. At least one parameter should be provided."
                    );
                }

                #region String Tests

                [Test]
                public static void OneEmptyStringShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('')",
                        ""
                    );
                }

                [Test]
                public static void MultipleEmptyStringsShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('', '', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('', '', '')",
                        ""
                    );
                }

                [Test]
                public static void OneStringShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('hello')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hello
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello')",
                        "hello"
                    );
                }

                [Test]
                public static void TwoStringsShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('hello', 'brave')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hellobrave
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello', 'brave')",
                        "hellobrave"
                    );
                }

                [Test]
                public static void ManyStringsShouldWork_1()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('hello', 'brave', 'new', 'world')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hellobravenewworld
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello', 'brave', 'new', 'world')",
                        "hellobravenewworld"
                    );
                }

                [Test]
                public static void ManyStringsShouldWork_2()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('hello', '', 'brave', '', 'new', '', 'world'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hellobravenewworld
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello', '', 'brave', '', 'new', '', 'world')",
                        "hellobravenewworld"
                    );
                }

                [Test]
                public static void NestedConcatShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat('hello', concat('brave', 'new'), 'world'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hellobravenewworld
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello', concat('brave', 'new'), 'world')",
                        "hellobravenewworld"
                    );
                }

                [Test]
                public static void CompoundStringTest1()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(toLower('ONE'), '-', toUpper('two'), '-', base64('three'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     one-TWO-dGhyZWU=
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(toLower('ONE'), '-', toUpper('two'), '-', base64('three'))",
                        "one-TWO-dGhyZWU="
                    );
                }

                #endregion

                #region Integer Tests

                [Test]
                public static void OneIntegerShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(100)]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     100200
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(100)",
                        "100"
                    );
                }

                [Test]
                public static void TwoIntegersShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(100, 200)]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     100200
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(100, 200)",
                        "100200"
                    );
                }

                #endregion

                #region Array Tests

                /// <summary>
                /// note - returns an empty array.
                /// </summary>
                [Test]
                public static void OneEmptyArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[concat(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      []
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(intersection(createArray('aaa'), createArray('bbb')))",
                        Array.Empty<object>()
                    );
                }

                /// <summary>
                /// note - returns an empty array.
                /// </summary>
                [Test]
                public static void MultipleEmptyArraysShouldWork()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[concat(intersection(createArray('aaa'), createArray('bbb')), intersection(createArray('ccc'), createArray('ddd')), intersection(createArray('eee'), createArray('fff')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      []
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(intersection(createArray('aaa'), createArray('bbb')), intersection(createArray('ccc'), createArray('ddd')), intersection(createArray('eee'), createArray('fff')))",
                        Array.Empty<object>()
                    );
                }

                [Test]
                public static void OneSingleStringItemtArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[concat(createArray('hello')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello'))",
                        new object[] { "hello" }
                    );
                }

                [Test]
                public static void TwoSingleStringItemArraysShouldWork()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[concat(createArray('hello'), createArray('brave'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello",
                    //   "brave"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello'), createArray('brave'))",
                        new object[] { "hello", "brave" }
                    );
                }

                [Test]
                public static void FourSingleStringItemArraysShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(createArray('hello'), createArray('brave'), createArray('new'), createArray('world'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello",
                    //   "brave",
                    //   "new",
                    //   "world"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(concat(createArray('hello'), createArray('brave'), createArray('new'), createArray('world')))",
                        new object[] { "hello", "brave", "new", "world" }
                    );
                }

                [Test]
                public static void OneMultipleStringItemArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(createArray('hello', 'brave'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello",
                    //   "brave"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello', 'brave'))",
                        new object[] { "hello", "brave" }
                    );
                }

                [Test]
                public static void TwoMultipleStringItemArraysShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(createArray('hello', 'brave'), createArray('new', 'world'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello",
                    //   "brave",
                    //   "new",
                    //   "world"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello', 'brave'), createArray('new', 'world'))",
                        new object[] { "hello", "brave", "new", "world" }
                    );
                }

                [Test]
                public static void EmptyArraysShouldBeOmitted()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(createArray('hello'), intersection(createArray('aaa'), createArray('bbb')), createArray('brave'), intersection(createArray('ccc'), createArray('ddd')), createArray('new'), intersection(createArray('eee'), createArray('fff')), createArray('world'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     hellobravenewworld
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello'), intersection(createArray('aaa'), createArray('bbb')), createArray('brave'), intersection(createArray('ccc'), createArray('ddd')), createArray('new'), intersection(createArray('eee'), createArray('fff')), createArray('world'))",
                        new object[] { "hello", "brave", "new", "world" }
                    );
                }

                [Test]
                public static void MixedItemArraysShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(createArray('hello', 100), createArray('new', 200))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "hello",
                    //   "brave",
                    //   "new",
                    //   "world"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(createArray('hello', 100), createArray('new', 200))",
                        new object[] { "hello", 100, "new", 200 }
                    );
                }

                #endregion

                [Test]
                public static void MixedPrimitiveItemsShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[concat(100, "aaa", 200)]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     100200
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(100, 'aaa', 200)",
                        "100aaa200"
                    );
                }

                [Test]
                public static void MixedArraysAndPrimitiveShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[concat(createArray('hello', 100), 200)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:57:57 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The provided parameters for language function 'concat' are invalid. Either all or none of the parameters must be an array. Please see https://aka.ms/arm-template-expressions/#concat for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "concat(createArray('hello', 100), 200)",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'concat'. Either all or none of the parameters must be an array."
                    );
                }

            }

            public static class DataUriTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:41:13 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#dataUri for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUri()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:41:42 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#dataUri for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUri('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(100)]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:application/json;charset=utf8;base64,MTAw
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(100)",
                        "data:application/json;charset=utf8;base64,MTAw"
                    );
                }

                /// <summary>
                /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-5
                /// </summary>
                [Test]
                public static void StringShouldConvertToDataUri_1()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri('Hello')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;charset=utf8;base64,SGVsbG8=
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri('Hello')",
                        "data:text/plain;charset=utf8;base64,SGVsbG8="
                    );
                }

                /// <summary>
                /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-5
                /// </summary>
                [Test]
                public static void StringShouldConvertToDataUri_2()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri('Hello')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;charset=utf8;base64,SGVsbG8=
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri('Hello, World!')",
                        "data:text/plain;charset=utf8;base64,SGVsbG8sIFdvcmxkIQ=="
                    );
                }

                /// <summary>
                /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
                /// </summary>
                [Test]
                public static void StringShouldConvertToDataUri_3()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri('GIF87a')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;charset=utf8;base64,R0lGODdh
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri('GIF87a')",
                        "data:text/plain;charset=utf8;base64,R0lGODdh"
                    );
                }

                /// <summary>
                /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
                /// </summary>
                [Test]
                public static void StringShouldConvertToDataUri_4()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri('the data:1234,5678')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;charset=utf8;base64,dGhlIGRhdGE6MTIzNCw1Njc4
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri('the data:1234,5678')",
                        "data:text/plain;charset=utf8;base64,dGhlIGRhdGE6MTIzNCw1Njc4"
                    );
                }

                public static void EmptyArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:application/json;charset=utf8;base64,W10=
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(intersection(createArray('aaa'), createArray('bbb')))",
                        "data:application/json;charset=utf8;base64,W10="
                    );
                }

                [Test]
                public static void ArrayOfIntegersShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(createArray(100, 200))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:application/json;charset=utf8;base64,WzEwMCwyMDBd
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(createArray(100, 200))",
                        "data:application/json;charset=utf8;base64,WzEwMCwyMDBd"
                    );
                }

                [Test]
                public static void ArrayOfStringsShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(createArray(''))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:application/json;charset=utf8;base64,WyJvbmUiLCJ0d28iXQ==
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(createArray('one', 'two'))",
                        "data:application/json;charset=utf8;base64,WyJvbmUiLCJ0d28iXQ=="
                    );
                }

                [Test]
                public static void MixedArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(createArray('one', 200))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:application/json;charset=utf8;base64,WzEwMCwyMDBd
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(createArray('one', 200))",
                        "data:application/json;charset=utf8;base64,WzEwMCwyMDBd"
                    );
                }

                [Test]
                public static void RoundtripWithIntegerShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(dataUriToString('data:application/json;base64,MTAw'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;charset=utf8;base64,MTAw
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(dataUriToString('data:application/json;base64,MTAw'))",
                        "data:application/json;charset=utf8;base64,MTAw"
                    );
                }

                [Test]
                public static void RoundtripWitStringShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUri(dataUriToString('data:text/plain;base64,YWFh'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     data:text/plain;base64,YWFh
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUri(dataUriToString('data:text/plain;base64,YWFh'))",
                        "data:text/plain;charset=utf8;base64,YWFh"
                    );
                }

            }

            public static class DataUriToStringTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:58:57 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:59:30 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:50:52 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void ArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString(createArray(''))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 08:51:35 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Array'. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString(createArray(''))",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Array'."
                    );
                }

                [Test]
                [Ignore("pending response to https://github.com/Azure/azure-powershell/issues/13179")]
                public static void InvalidDataPrefixShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('invalid datauri')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:01:07 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'invalid datauri' was not formatted correctly. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString('invalid datauri')",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'invalid datauri' was not formatted correctly."
                    );
                }

                [Test]
                [Ignore("pending response to https://github.com/Azure/azure-powershell/issues/13179")]
                public static void InvalidContentShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:invalid datauri')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:02:05 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'data:invalid datauri' was not formatted correctly. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString('data:invalid datauri')",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'data:invalid datauri' was not formatted correctly."
                    );
                }

                [Test]
                [Ignore("pending response to https://github.com/Azure/azure-powershell/issues/13179")]
                public static void ValidDataUriButUnsupportedCharsetShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:application/json;charset=utf8;base64,MTAw')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:02:37 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' parameter is not valid. The provided charset 'utf8' is not supported. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString('data:application/json;charset=utf8;base64,MTAw')",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' parameter is not valid. The provided charset 'utf8' is not supported."
                    );
                }

                /// <summary>
                /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
                /// </summary>
                [Test]
                public static void ValidDataUriButUnsupportedParametersShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:;foo=bar;base64,R0lGODdh')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:03:58 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'data:;foo=bar;base64,R0lGODdh' was not formatted correctly. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString('data:;foo=bar;base64,R0lGODdh')",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'data:;foo=bar;base64,R0lGODdh' was not formatted correctly."
                    );
                }

                /// <summary>
                /// This is possibly a bug in the ARM Template Deployment api -
                /// dataUri uses "charset=utf8", but dataUriToString expects "charset=UTF-8"
                /// </summary>
                [Test]
                [Ignore("pending response to https://github.com/Azure/azure-powershell/issues/13179")]
                public static void RoundtripWithIntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString(dataUri(100))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:05:32 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'dataUriToString' parameter is not valid. The provided charset 'utf8' is not supported. Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUriToString(dataUri(100))",
                        typeof(InvalidOperationException),
                        "The template language function 'dataUriToString' parameter is not valid. The provided charset 'utf8' is not supported."
                    );
                }

                [Test]
                public static void ShouldConvertToIntegerWithNoCharset()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:application/json;base64,MTAw')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:application/json;base64,MTAw')",
                        100
                    );
                }

                [Test]
                public static void ShouldConvertToIntegerWithCharset()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:application/json;charset=UTF-8;base64,MTAw')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:application/json;charset=UTF-8;base64,MTAw')",
                        100
                    );
                }

                /// <summary>
                /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
                /// </summary>
                [Test]
                public static void ShouldConvertToString_1()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:;base64,SGVsbG8=')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     Hello
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:;base64,SGVsbG8=')",
                        "Hello"
                    );
                }

                /// <summary>
                /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
                /// </summary>
                [Test]
                public static void ShouldConvertToString_2()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:;base64,SGVsbG8sIFdvcmxkIQ==')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     Hello, World!
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:;base64,SGVsbG8sIFdvcmxkIQ==')",
                        "Hello, World!"
                    );
                }

                /// <summary>
                /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
                /// </summary>
                [Test]
                public static void ShouldConvertToString_3()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:;base64,R0lGODdh')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     GIF87a
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:;foo=bar;base64,R0lGODdh')",
                        "GIF87a"
                    );
                }

                /// <summary>
                /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
                /// </summary>
                [Test]
                public static void ShouldConvertToString_4()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[dataUriToString('data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     the data:1234,5678
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678')",
                        "the data:1234,5678"
                    );
                }

            }

            public static class EmptyTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:25:46 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'empty': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#empty for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "empty()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'empty': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:26:35 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'empty': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#empty for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "empty('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'empty': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:27:18 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template function 'empty' expects its parameter to be an object, an array, or a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#empty for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "empty(100)",
                        typeof(InvalidOperationException),
                        "The template function 'empty' expects its parameter to be an object, an array, or a string. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void EmptyArrayShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty(intersection(createArray('aaa'), createArray('bbb')))",
                        true
                    );
                }

                [Test]
                public static void ArrayWithValuesShouldReturnFalse()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty(createArray('one'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty(createArray('one'))",
                        false
                    );
                }

                [Test]
                public static void EmptyStringShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[empty('')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty('')",
                        true
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFalse()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[empty('abc')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty('abc')",
                        false
                    );
                }

            }

            public static class EndsWithTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 20:59:48 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#endsWith for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "endsWith()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[empty('one', 'two', 'three')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:00:59 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 3 were provided. Please see https://aka.ms/arm-template-expressions/#endsWith for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "endsWith('one', 'two', 'three')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 3 were provided."
                    );
                }

                [Test]
                public static void IntegersShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith(100, 200)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:02:03 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'. Please see https://aka.ms/arm-template-expressions#endsWith for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "endsWith(100, 200)",
                        typeof(InvalidOperationException),
                        "The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'."
                    );
                }

                [Test]
                public static void ArraysShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith(createArray('one'), createArray('two'))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 09:33:56 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'. Please see https://aka.ms/arm-template-expressions#endsWith for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "endsWith(createArray('one'), createArray('two'))",
                        typeof(InvalidOperationException),
                        "The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'."
                    );
                }

                [Test]
                public static void StringAndIntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith("one", 100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 15:14:15 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'String' and 'Integer'. Please see https://aka.ms/arm-template-expressions#endsWith for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "endsWith('one', 100)",
                        typeof(InvalidOperationException),
                        "The template language function 'endsWith' expects its parameters to be of type string and string. The provided value is of type 'String' and 'Integer'."
                    );
                }

                [Test]
                public static void MatchingCaseShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith('abcdef', 'ef')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'ef')",
                        true
                    );
                }

                [Test]
                public static void CaseSensitiveMatchShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith('abcdef', 'F')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'F')",
                        true
                    );
                }

                [Test]
                public static void NoMatchShouldReturnFalse()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith('abcdef', 'e')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       False
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'e')",
                        false
                    );
                }

                [Test]
                public static void EmptyStringEndsWithEmptyStringShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith('', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('', '')",
                        true
                    );
                }

                [Test]
                public static void StringValueEndsWithEmptyStringShouldReturnTrue()
                {
                    // "test-output": {
                    //   "type": "bool",
                    //   "value": "[endsWith('abcdef', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', '')",
                        true
                    );
                }

            }

            public static class FirstTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:17:34 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'first': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#first for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "first()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'first': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:18:13 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'first': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#first for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "first('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'first': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:19:52 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'first' expects its parameter be an array or a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#first for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "first(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'first' expects its parameter be an array or a string. The provided value is of type 'Integer'."
                    );
                }

                /// <summary>
                /// This is possibly a bug in the ARM Template API.
                /// </summary>
                [Test]
                public static void EmptyArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 10:34:47 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Template output JToken type is not valid. Expected 'Integer'. Actual 'Null'.Please see https://aka.ms/arm-template/#outputs for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "first(intersection(createArray('aaa'), createArray('bbb')))",
                        typeof(InvalidOperationException),
                        "Template output JToken type is not valid. Expected 'Integer'. Actual 'Null'."
                    );
                }

                [Test]
                public static void SingleItemIntgerArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[first(createArray(100))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first(createArray(100))",
                        100
                    );
                }

                [Test]
                public static void MultiItemIntegerArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[first(createArray(100, 200))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first(createArray(100, 200))",
                        100
                    );
                }

                [Test]
                public static void SingleItemStringArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first(createArray('aaa'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first(createArray('aaa'))",
                        "aaa"
                    );
                }

                [Test]
                public static void MultiItemStringArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first(createArray('aaa', 'bbb'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first(createArray('aaa', 'bbb'))",
                        "aaa"
                    );
                }

                [Test]
                public static void MixedValueArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first(createArray('aaa', 100))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first(createArray('aaa', 100))",
                        "aaa"
                    );
                }

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first('')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[first('One Two Three')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     O
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first('One Two Three')",
                        "O"
                    );
                }

            }

            public static class FormatTests
            {

                [Test]
                public static void StringShouldFormat()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "format('{0}, {1}. Formatted number: {2:N0}', 'Hello', 'User', 8175133)",
                        "Hello, User. Formatted number: 8,175,133"
                    );
                }

            }

            public static class IndexOfTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:31:34 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#indexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "indexOf()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('one', 'two', 'three')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:33:13 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 3 were provided. Please see https://aka.ms/arm-template-expressions/#indexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "indexOf('one', 'two', 'three')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 3 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf(100, 200)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:34:00 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'indexOf' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'. Please see https://aka.ms/arm-template-expressions#indexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "indexOf(100, 200)",
                        typeof(InvalidOperationException),
                        "The template language function 'indexOf' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'."
                    );
                }

                [Test]
                public static void ArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf(createArray('aaa'), createArray('bbb'))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 11:49:52 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'indexOf' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'. Please see https://aka.ms/arm-template-expressions#indexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "indexOf(createArray('aaa'), createArray('bbb'))",
                        typeof(InvalidOperationException),
                        "The template language function 'indexOf' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'."
                    );
                }

                [Test]
                public static void EmptyStringInEmptyStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        0
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringInStringValueShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('abcdefg', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        0
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', '')",
                        0
                    );
                }

                [Test]
                public static void MatchAtStartOfStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('test', 't')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        0
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('test', 't')",
                        0
                    );
                }

                [Test]
                public static void CaseInsensitiveMatchShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('abcdef', 'CD')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        2
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'CD')",
                        2
                    );
                }

                [Test]
                public static void NoMatchShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[indexOf('abcdef', 'z')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        -1
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LastTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:43:42 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'last': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#last for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "last()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'last': function requires 1 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:45:00 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'last': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#last for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "last('one', 'two')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'last': function requires 1 argument(s) while 2 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:45:47 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'last' expects its parameter be an array or a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#last for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "last(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'last' expects its parameter be an array or a string. The provided value is of type 'Integer'."
                    );
                }

                /// <summary>
                /// This is possibly a bug in the ARM Template API.
                /// </summary>
                [Test]
                public static void EmptyArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 11:53:10 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Template output JToken type is not valid. Expected 'String'. Actual 'Null'.Please see https://aka.ms/arm-template/#outputs for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "last(intersection(createArray('aaa'), createArray('bbb')))",
                        typeof(InvalidOperationException),
                        "Template output JToken type is not valid. Expected 'Integer'. Actual 'Null'."
                    );
                }

                [Test]
                public static void SingleItemIntgerArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[last(createArray(100))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last(createArray(100))",
                        100
                    );
                }

                [Test]
                public static void MultiItemIntegerArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[last(createArray(100, 200))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Integer                    100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last(createArray(100, 200))",
                        200
                    );
                }

                [Test]
                public static void SingleItemStringArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last(createArray('aaa'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last(createArray('aaa'))",
                        "aaa"
                    );
                }

                [Test]
                public static void MultiItemStringArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last(createArray('aaa', 'bbb'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last(createArray('aaa', 'bbb'))",
                        "bbb"
                    );
                }

                [Test]
                public static void MixedValueArrayShouldWork()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[last(createArray('aaa', 100))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     aaa
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last(createArray('aaa', 100))",
                        100
                    );
                }

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last('')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[last('One Two Three')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     e
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('One Two Three')",
                        "e"
                    );
                }

            }

            public static class LastIndexOfTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:50:56 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#lastIndexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "lastIndexOf()",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 0 were provided."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('one', 'two', 'three')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:53:11 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 3 were provided. Please see https://aka.ms/arm-template-expressions/#lastIndexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "lastIndexOf('one', 'two', 'three')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 3 were provided."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf(100, 200)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:54:10 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'lastIndexOf' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'. Please see https://aka.ms/arm-template-expressions#lastIndexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "lastIndexOf(100, 200)",
                        typeof(InvalidOperationException),
                        "The template language function 'lastIndexOf' expects its parameters to be of type string and string. The provided value is of type 'Integer' and 'Integer'."
                    );
                }

                [Test]
                public static void ArrayShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf(createArray('aaa'), createArray('bbb'))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 14:34:00 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'lastIndexOf' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'. Please see https://aka.ms/arm-template-expressions#lastIndexOf for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "lastIndexOf(createArray('aaa'), createArray('bbb'))",
                        typeof(InvalidOperationException),
                        "The template language function 'lastIndexOf' expects its parameters to be of type string and string. The provided value is of type 'Array' and 'Array'."
                    );
                }

                [Test]
                public static void EmptyStringInEmptyStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        0
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringInStringValueShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('abcdefg', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        0
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', '')",
                        5
                    );
                }

                [Test]
                public static void MatchAtEndOfStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('test', 't')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        3
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('test', 't')",
                        3
                    );
                }

                [Test]
                public static void CaseInsensitiveMatchShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('abcdef', 'CD')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        2
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', 'CD')",
                        2
                    );
                }

                [Test]
                public static void NoMatchShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[lastIndexOf('abcdef', 'z')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Int                        -1
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LengthTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 16:42:23 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. The function was invoked with '0' parameters. Please see https://aka.ms/arm-template-expressions/#length for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "length()",
                        typeof(InvalidOperationException),
                        "The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. The function was invoked with '0' parameters."
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length('one', 'two')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 16:43:22 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. The function was invoked with '2' parameters. Please see https://aka.ms/arm-template-expressions/#length for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "length('one', 'two')",
                        typeof(InvalidOperationException),
                        "The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. The function was invoked with '2' parameters."
                    );
                }

                [Test]
                public static void IntegerShouldThrow()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length(100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 16:44:30 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'length' expects its parameter to be an array, object, or a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions/#length for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "length(100)",
                        typeof(InvalidOperationException),
                        "The template language function 'length' expects its parameter to be of type 'String'. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void EmptyArrayShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length(intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length(intersection(createArray('aaa'), createArray('bbb')))",
                        0
                    );
                }

                [Test]
                public static void ArrayWithSingleValusShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length(createArray('one'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length(createArray('one'))",
                        1
                    );
                }

                public static void ArrayWithMultipleValuesShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length(createArray('one', 'two'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length(createArray('one', 'two'))",
                        2
                    );
                }

                [Test]
                public static void EmptyStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length('')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length('')",
                        0
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturn()
                {
                    // "test-output": {
                    //   "type": "int",
                    //   "value": "[length('abc')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Bool                       True
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length('abc')",
                        3
                    );
                }

            }

            public static class PadLeftTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 20:53:02 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': function requires minimum two and maximum three arguments while '0' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft()",
                        typeof(InvalidOperationException),
                        "padLeft': function requires minimum two and maximum three arguments while '0' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])"
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft('one', 'two', 'three', 'four')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 20:54:06 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': function requires minimum two and maximum three arguments while '4' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft('one', 'two', 'three', 'four')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': function requires minimum two and maximum three arguments while '4' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])"
                    );
                }

                [Test]
                public static void IntegerValueShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(100, 10, '0')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     0000000100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft(100, 10, '0')",
                        "0000000100"
                    );
                }

                [Test]
                public static void NegativeIntegerValueShouldWork()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(-100, 10, '0')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     000000-100
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft(-100, 10, '0')",
                        "000000-100"
                    );
                }

                [Test]
                public static void ArrayValueShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(createArray(''), 10, '0')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:12:01 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the first parameter is invalid. The source string must be a String or integer type, while 'Array' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft(createArray(''), 10, '0')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the first parameter is invalid. The source string must be a String or integer type, while 'Array' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])."
                    );
                }

                [Test]
                public static void LargeTotalWidthShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(100, 200)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 20:55:39 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the second parameter is invalid. Total width must be a positive integer value and not greater than '16', while '200' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft(100, 200)",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the second parameter is invalid. Total width must be a positive integer value and not greater than '16', while '200' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])."
                    );
                }

                [Test]
                public static void IntegerPaddingCharShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(10, 15, 30)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 20:58:48 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a string type, while 'Integer' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft(10, 15, 30)",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a string type, while 'Integer' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])."
                    );
                }

                [Test]
                public static void EmptyPaddingShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(10, 15, '')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:02:18 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a single character, while '' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft(10, 15, 'abc')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a single character, while 'abc' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])"
                    );
                }

                [Test]
                public static void MulticharPaddingShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft(10, 15, '')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:00:42 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a single character, while 'abc' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft(10, 15, 'abc')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the third parameter is invalid. The padding character must be a single character, while 'abc' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])"
                    );
                }

                [Test]
                public static void DefaultPaddingCharShouldBeSpace()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft('123', 10)]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                            123
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10)",
                        "       123"
                    );
                }

                [Test]
                public static void PaddingCharShouldBeUsedWhenSpecified()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft('123', 10. '0')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     0000000123
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10, '0')",
                        "0000000123"
                    );
                }

                [Test]
                public static void ShortStringShouldNotAppendPadding()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft('123', 2. '0')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      String                     123
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 2, '0')",
                        "123"
                    );
                }

                [Test]
                public static void NegativePaddingShouldThrow()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[padLeft('123', -1, '0')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:05:06 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: Unable to evaluate template language function 'padLeft': the second parameter is invalid. Total width must be a positive integer value and not greater than '16', while '-1' was provided. The syntax is padLeft(string, totalWidth [, paddingChar]). Please see https://aka.ms/arm-template-expressions/#padleft for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "padLeft('123', -1, '0')",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'padLeft': the second parameter is invalid. Total width must be a positive integer value and not greater than '16', while '-1' was provided. The syntax is padLeft(string, totalWidth [, paddingChar])"
                    );
                }

            }

            public static class ReplaceTests
            {

                [Test]
                public static void ShouldReplaceWithEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "replace('123-123-1234', '-', '')",
                        "1231231234"
                    );
                }

                [Test]
                public static void ShouldReplaceSingleMatch()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "replace('123-123-1234', '1234', 'xxxx')",
                        "123-123-xxxx"
                    );
                }

            }

            public static class SkipTests
            {

                [Test]
                public static void ShouldSkipEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('', 5)",
                        ""
                    );
                }

                [Test]
                public static void ShouldSkipCharacters()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('one two three', 4)",
                        "two three"
                    );
                }

                [Test]
                public static void ShouldSkipWithZeroLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('one two three', 0)",
                        "one two three"
                    );
                }


                [Test]
                public static void ShouldSkipWithNegativeLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('one two three', -100)",
                        "one two three"
                    );
                }

                [Test]
                public static void ShouldSkipWithExactStringLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('one two three', 13)",
                        ""
                    );
                }

                [Test]
                public static void ShouldSkipIfPastEndOfString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "skip('one two three', 100)",
                        ""
                    );
                }

            }

            public static class SplitTests
            {

                [Test]
                public static void NoArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split()]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 22:09:21 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects exactly '2' parameters. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split()",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n"
                    );
                }

                [Test]
                public static void OneArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 22:09:59 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects exactly '2' parameters. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split('one')",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.String"
                    );
                }

                [Test]
                public static void TooManyArgumentsShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one', 'two', 'three')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 22:13:11 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects exactly '2' parameters. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split('one', 'two', 'three')",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.String\r\n" +
                        "System.String\r\n" +
                        "System.String"
                    );
                }

                [Test]
                public static void IntegerValueShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split(100, ',')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 22:14:38 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects its first parameter to be of type 'String'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split(100, ',')",
                        typeof(InvalidOperationException),
                        "The template language function 'split' expects its first parameter to be of type 'String'. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void IntegerDelimiterShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one,two,three', 100)]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:17:58 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects its second parameter to be of type 'String or Array of Strings'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split('one,two,three', 100)",
                        typeof(InvalidOperationException),
                        "The template language function 'split' expects its second parameter to be of type 'String or Array of Strings'. The provided value is of type 'Integer'."
                    );
                }

                [Test]
                public static void ArrayValueShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split(createArray(''), ',')]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 21:20:17 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects its first parameter to be of type 'String'. The provided value is of type 'Array'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split(createArray(''), ',')",
                        typeof(InvalidOperationException),
                        "The template language function 'split' expects its first parameter to be of type 'String'. The provided value is of type 'Array'."
                    );
                }

                [Test]
                public static void ShouldSplitStringDelimiter()
                {
                    // "test-output": {
                    //   "type": "string",
                    //   "value": "[split('One Two Three', ',')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "One Two Three"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two,three', ',')",
                        new string[] { "one", "two", "three" }
                    );
                }

                [Test]
                public static void ShouldSplitArrayDelimiterWithStrings()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one,two;three', createArray(',', ';'))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //   "one",
                    //   "two",
                    //   "three"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two;three', createArray(',', ';'))",
                        new string[] { "one", "two", "three" }
                    );
                }

                [Test]
                public static void ArrayDelimiterWithIntegersShouldThrow()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one,two;three', createArray(100, 200))]"
                    // },
                    //
                    // New-AzResourceGroupDeployment: 22:46:37 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: The template output 'test-output' is not valid: The template language function 'split' expects its second parameter to be of type 'String or Array of Strings'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "split('one,two;three', createArray(100, 200))",
                        typeof(InvalidOperationException),
                        "Unable to evaluate template language function 'split'. The template language function 'split' expects its second parameter to be of type 'String or Array of Strings'"
                    );
                }

                [Test]
                public static void ShouldSplitEmptyStringAndEmptyDelimiter()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('', '')]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //  ""
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('', '')",
                        new string[] { "" }
                    );
                }

                [Test]
                public static void ShouldSplitStringAndArrayDelimiter()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one,two,three', createArray(''))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //  "one,two,three"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two,three', createArray(''))",
                        new string[] { "" }
                    );
                }

                [Test]
                public static void ShouldSplitStringAndEmptyArrayDelimiter()
                {
                    // "test-output": {
                    //   "type": "array",
                    //   "value": "[split('one,two,three', intersection(createArray('aaa'), createArray('bbb')))]"
                    // },
                    //
                    // Name             Type                       Value
                    // ===============  =========================  ==========
                    // test-output      Array                      [
                    //  "one,two,three"
                    // ]
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two,three', intersection(createArray('aaa'), createArray('bbb')))",
                        new string[] { "" }
                    );
                }

            }

            public static class StartsWithTests
            {

                [Test]
                public static void MatchingCaseShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'ab')",
                        true
                    );
                }

                [Test]
                public static void CaseSensitiveMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'A')",
                        true
                    );
                }

                [Test]
                public static void NoMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'e')",
                        false
                    );
                }

            }

            public static class SubstringTests
            {

                [Test]
                public static void ShouldReturnSubstring()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "substring('one two three', 4, 3)",
                        "two"
                    );
                }

            }

            public static class TakeTests
            {

                [Test]
                public static void ShouldTakeEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('', 5)",
                        ""
                    );
                }

                [Test]
                public static void ShouldTakeCharacters()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('one two three', 2)",
                        "on"
                    );
                }

                [Test]
                public static void ShouldTakeWithZeroLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('one two three', 0)",
                        ""
                    );
                }


                [Test]
                public static void ShouldTakeWithNegativeLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('one two three', -100)",
                        ""
                    );
                }

                [Test]
                public static void ShouldTakeWithExactStringLength()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('one two three', 13)",
                        "one two three"
                    );
                }

                [Test]
                public static void ShouldTakeIfPastEndOfString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "take('one two three', 100)",
                        "one two three"
                    );
                }

            }

            public static class ToLowerTests
            {

                [Test]
                public static void SampleStringShouldConvertToLower()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toLower('One Two Three')",
                        "one two three"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toLower(toUpper('one two three'))",
                        "one two three"
                    );
                }

            }

            public static class ToUpperTests
            {

                [Test]
                public static void SampleStringShouldConvertToUpper()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toUpper('One Two Three')",
                        "ONE TWO THREE"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toUpper(toLower('ONE TWO THREE'))",
                        "ONE TWO THREE"
                    );
                }

            }

            public static class TrimTests
            {

                [Test]
                public static void ShouldTrimEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "trim('')",
                        ""
                    );
                }

                [Test]
                public static void ShouldTrimString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "trim('    one two three   ')",
                        "one two three"
                    );
                }

            }

        }

    }

}
