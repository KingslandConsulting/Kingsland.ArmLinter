using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class ToLowerTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toLower()",
                    typeof(ArgumentException),
                    "The template language function 'toLower' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toLower('one', 'two')",
                    typeof(ArgumentException),
                    "The template language function 'toLower' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toLower(100)",
                    typeof(ArgumentException),
                    "The template language function 'toLower' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toLower(createArray(''))",
                    typeof(ArgumentException),
                    "The template language function 'toLower' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void StringShouldConvertToLower()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "toLower('One Two Three')",
                    "one two three"
                );
            }

            [Test]
            public static void StringShouldRoundtrip()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "toLower(toUpper('one two three'))",
                    "one two three"
                );
            }

        }

    }

}
