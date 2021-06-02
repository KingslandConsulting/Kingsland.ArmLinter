using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class DataUriToStringTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString()",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 0 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#dataUriToString for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString('one', 'two')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'dataUriToString': function requires 1 argument(s) while 2 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#dataUriToString for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString(100)",
                    typeof(ArgumentException),
                    $"The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString(createArray(''))",
                    typeof(ArgumentException),
                    $"The template language function 'dataUriToString' expects its parameter to be a string. The provided value is of type 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details."
                );
            }

            [Test]
            [Ignore("pending response to https://github.com/Azure/azure-powershell/issues/13179")]
            public static void InvalidDataPrefixShouldThrow()
            {
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
            [Ignore("behaviour not implemented yet")]
            public static void ValidDataUriButUnsupportedParametersShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString('data:;foo=bar;base64,R0lGODdh')",
                    typeof(InvalidOperationException),
                    $"The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. The provided value 'data:;foo=bar;base64,R0lGODdh' was not formatted correctly. " +
                    $"Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details."
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
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString(dataUri(100))",
                    typeof(InvalidOperationException),
                    "The template language function 'dataUriToString' parameter is not valid. The provided charset 'utf8' is not supported."
                );
            }

            [Test]
            public static void ShouldConvertToIntegerWithNoCharset()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUriToString('data:application/json;base64,MTAw')",
                    "100"
                );
            }

            [Test]
            public static void ShouldConvertToIntegerWithCharset()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUriToString('data:application/json;charset=UTF-8;base64,MTAw')",
                    "100"
                );
            }

            /// <summary>
            /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
            /// </summary>
            [Test]
            public static void ShouldConvertToString_1()
            {
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
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUriToString('data:;base64,SGVsbG8sIFdvcmxkIQ==')",
                    "Hello, World!"
                );
            }

            /// <summary>
            /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
            /// </summary>
            [Test]
            [Ignore("behaviour not implemented yet")]
            public static void ShouldConvertToString_3()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUriToString('data:;foo=bar;base64,R0lGODdh')",
                    typeof(InvalidOperationException),
                    "The template language function 'dataUriToString' expects its parameter to be formatted as a valid data URI. " +
                    "The provided value 'data:;foo=bar;base64,R0lGODdh' was not formatted correctly. " +
                    "Please see https://aka.ms/arm-template-expressions#dataUriToString for usage details."
                );
            }

            /// <summary>
            /// Example from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax
            /// </summary>
            [Test]
            public static void ShouldConvertToString_4()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUriToString('data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678')",
                    "the data:1234,5678"
                );
            }

        }

    }

}
