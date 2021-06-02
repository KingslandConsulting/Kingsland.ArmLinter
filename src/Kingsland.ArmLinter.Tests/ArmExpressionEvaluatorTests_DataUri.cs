using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class DataUriTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUri()",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 0 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#dataUri for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "dataUri('one', 'two')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'dataUri': function requires 1 argument(s) while 2 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#dataUri for usage details."
                );
            }

            [Test]
            public static void IntegerShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(100)",
                    "data:application/json;charset=utf8;base64,MTAw"
                );
            }

            [Test]
            public static void LongShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    $"dataUri(2147483647)",
                    $"data:application/json;charset=utf8;base64,MjE0NzQ4MzY0Nw=="
                );
            }

            [Test]
            [Ignore("not handling integer overflows right now")]
            public static void BigLongShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    $"dataUri(2147483648)",
                    typeof(InvalidOperationException),
                    $"Microsoft.Rest.Azure.CloudException: " +
                    $"Deployment template language expression evaluation failed: " +
                    "'The language expression 'dataUri(2147483648)' is not valid: " +
                    "the value '2147483648' at position '8' cannot be converted to number.'" +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            /// <summary>
            /// Example from https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-5
            /// </summary>
            [Test]
            public static void StringShouldConvertToDataUri_1()
            {
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
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri('the data:1234,5678')",
                    "data:text/plain;charset=utf8;base64,dGhlIGRhdGE6MTIzNCw1Njc4"
                );
            }

            public static void EmptyArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(intersection(createArray('aaa'), createArray('bbb')))",
                    "data:application/json;charset=utf8;base64,W10="
                );
            }

            [Test]
            public static void ArrayOfIntegersShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(createArray(100, 200))",
                    "data:application/json;charset=utf8;base64,WzEwMCwyMDBd"
                );
            }

            [Test]
            public static void ArrayOfStringsShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(createArray('one', 'two'))",
                    "data:application/json;charset=utf8;base64,WyJvbmUiLCJ0d28iXQ=="
                );
            }

            [Test]
            public static void MixedArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(createArray('one', 200))",
                    "data:application/json;charset=utf8;base64,WyJvbmUiLDIwMF0="
                );
            }

            [Test]
            public static void RoundtripWithIntegerShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(dataUriToString('data:application/json;base64,MTAw'))",
                    "data:text/plain;charset=utf8;base64,MTAw"
                );
            }

            [Test]
            public static void RoundtripWithStringShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "dataUri(dataUriToString('data:text/plain;base64,YWFh'))",
                    "data:text/plain;charset=utf8;base64,YWFh"
                );
            }

        }

    }

}
