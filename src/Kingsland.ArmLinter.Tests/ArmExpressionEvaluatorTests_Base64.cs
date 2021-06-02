using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class Base64Tests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64()",
                    typeof(ArgumentException),
                    "The template language function 'base64' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64('one', 'two')",
                    typeof(ArgumentException),
                    "The template language function 'base64' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64(100)",
                    typeof(ArgumentException),
                    "The template language function 'base64' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "base64(createArray(''))",
                    typeof(ArgumentException),
                    "The template language function 'base64' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void StringShouldEncodeToBase64()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "base64('one, two, three')",
                    "b25lLCB0d28sIHRocmVl"
                );
            }

            [Test]
            public static void StringShouldRoundtrip()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "base64ToString(base64('one, two, three'))",
                    "one, two, three"
                );
            }

        }

    }

}
