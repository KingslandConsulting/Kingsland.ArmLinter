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
                public static void InvokingWithNoArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:39:23 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-deployment' is not valid: The template language function 'base64' must have only one parameter. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64()",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n"
                    );
                }

                [Test]
                public static void InvokingWithTooManyArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:41:30 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-deployment' is not valid: The template language function 'base64' must have only one parameter.Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64('one', 'two')",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.String\r\n" +
                        "System.String"
                    );
                }

                [Test]
                public static void InvokingWithIntegerShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:51:10 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: The template language function 'base64' expects its parameter to be of type 'String'. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64(100)",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.Int32"
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
                public static void InvokingWithNoArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:43:56 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test' is not valid: Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString()",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n"
                    );
                }

                [Test]
                public static void InvokingWithTooManyArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:45:42 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'.Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString('one', 'two')",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.String\r\n" +
                        "System.String"
                    );
                }

                [Test]
                public static void InvokingWithIntegerShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:55:52 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: The template language function 'base64ToString' expects its parameter to be a string. The provided value is of type 'Integer'. Please see https://aka.ms/arm-template-expressions#base64ToString for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "base64ToString(100)",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.Int32"
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
                public static void InvokingWithNoArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 20:59:20 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: Unable to evaluate template language function 'concat'. At least one parameter should be provided. Please see https://aka.ms/arm-template-expressions for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "concat()",
                        typeof(InvalidOperationException),
                        "More than one method overload matches the arguments.\r\n" +
                        "\r\n" +
                        "Overloads are:\r\n" +
                        "System.String Concat(System.String[])\r\n" +
                        "System.Object[] Concat(System.Object[][])\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n"
                    );
                }

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
                public static void CompoundTest1()
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

            }

            public static class DataUriTests
            {

                [Test]
                public static void InvokingWithNoArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 21:32:14 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 0 were provided. Please see https://aka.ms/arm-template-expressions/#dataUri for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUri()",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n"
                    );
                }

                [Test]
                public static void InvokingWithTooManyArgumentsShouldThrow()
                {
                    // New-AzResourceGroupDeployment : 21:35:34 - The deployment 'arm_functions' failed with error(s). Showing 1 out of 1 error(s).
                    // Status Message: Unable to evaluate template outputs: 'test-output'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details. (Code: DeploymentOutputEvaluationFailed)
                    //  - The template output 'test-output' is not valid: Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 2 were provided. Please see https://aka.ms/arm-template-expressions/#dataUri for usage details.. (Code:DeploymentOutputEvaluationFailed)
                    ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                        "dataUri('one', 'two')",
                        typeof(InvalidOperationException),
                        "No method overloads match the arguments.\r\n" +
                        "\r\n" +
                        "Arguments are:\r\n" +
                        "System.String\r\n" +
                        "System.String"
                    );
                }

                [Test]
                public static void IntegerShouldConvertToDataUri()
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
                /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
                /// </summary>
                [Test]
                public static void StringShouldConvertToDataUri_2()
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
                public static void StringShouldConvertToDataUri_3()
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

            }

            public static class DataUriToStringTests
            {

                [Test]
                public static void ShouldConvertFromDataUri_1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:;base64,SGVsbG8sIFdvcmxkIQ==')",
                        "Hello, World!"
                    );
                }

                [Test]
                public static void ShouldConvertFromDataUri_2()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:text/vnd-example+xyz;foo=bar;base64,R0lGODdh')",
                        "GIF87a"
                    );
                }

                [Test]
                public static void ShouldConvertFromDataUri_3()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "dataUriToString('data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678')",
                        "the data:1234,5678"
                    );
                }

            }

            public static class EmptyTests
            {

                [Test]
                public static void EmptyStringShouldReturnTrue()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty('')",
                        true
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFalse()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "empty('abc')",
                        false
                    );
                }

            }

            public static class EndsWithTests
            {

                [Test]
                public static void MatchingCaseShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'ef')",
                        true
                    );
                }

                [Test]
                public static void CaseSensitiveMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'F')",
                        true
                    );
                }

                [Test]
                public static void NoMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'e')",
                        false
                    );
                }

            }

            public static class FirstTests
            {

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
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
                public static void EmptyStringShouldReturn_1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringShouldReturn_2()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', '')",
                        0
                    );
                }

                [Test]
                public static void MatchAtStartShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('test', 't')",
                        0
                    );
                }

                [Test]
                public static void MatchShouldBeCaseInsensitive()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'CD')",
                        2
                    );
                }

                [Test]
                public static void NotFoundSHouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LastTests
            {

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('One Two Three')",
                        "e"
                    );
                }

            }

            public static class LastIndexOfTests
            {

                [Test]
                public static void EmptyStringShouldReturn_1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringShouldReturn_2()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', '')",
                        5
                    );
                }

                [Test]
                public static void MatchAtEndShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('test', 't')",
                        3
                    );
                }

                [Test]
                public static void MatchShouldBeCaseInsensitive()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', 'AB')",
                        0
                    );
                }

                [Test]
                public static void NotFoundShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LengthTests
            {

                [Test]
                public static void EmptyStringShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length('')",
                        0
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "length('One Two Three')",
                        13
                    );
                }

            }

            public static class PadLeftTests
            {

                [Test]
                public static void SampleStringShouldPadLeft()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10, '0')",
                        "0000000123"
                    );
                }

                [Test]
                public static void ShortStringShouldNotAppendPadding()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 2, '0')",
                        "123"
                    );
                }

                [Test]
                public static void DefaultPaddingCharacterShouldBeSpace()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10)",
                        "       123"
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
                public static void ShouldSplitOneDelimiter()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two,three', ',')",
                        new string[] { "one", "two", "three" }
                    );
                }

                [Test]
                public static void ShouldSplitMultipleDelimiter()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two;three', createArray(',', ';'))",
                        new string[] { "one", "two", "three" }
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
