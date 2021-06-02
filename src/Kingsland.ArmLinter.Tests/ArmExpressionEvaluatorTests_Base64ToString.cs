using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class Base64ToStringTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64ToString()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 0 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64ToString('one', 'two')",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'base64ToString': function requires 1 argument(s) while 2 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#base64ToString for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64ToString(100)",
                    typeof(ArgumentException),
                    "The template language function 'base64ToString' expects its parameter to be a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#base64ToString for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64ToString(createArray(''))",
                    typeof(ArgumentException),
                    "The template language function 'base64ToString' expects its parameter to be a string. " +
                    "The provided value is of type 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions#base64ToString for usage details."
                );
            }

            [Test]
            public static void Base64StringShouldDecodeToOriginalString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "base64ToString('b25lLCB0d28sIHRocmVl')",
                    "one, two, three"
                );
            }

            [Test]
            public static void StringShouldRoundtrip()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "base64(base64ToString('b25lLCB0d28sIHRocmVl'))",
                    "b25lLCB0d28sIHRocmVl"
                );
            }

        }

    }

}
