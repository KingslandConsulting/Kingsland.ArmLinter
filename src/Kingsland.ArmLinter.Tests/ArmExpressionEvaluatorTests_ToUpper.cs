using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class ToUpperTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toUpper()",
                    typeof(ArgumentException),
                    "The template language function 'toUpper' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toUpper('one', 'two')",
                    typeof(ArgumentException),
                    "The template language function 'toUpper' must have only one parameter. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toUpper(100)",
                    typeof(ArgumentException),
                    "The template language function 'toUpper' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "toUpper(createArray(''))",
                    typeof(ArgumentException),
                    "The template language function 'toUpper' expects its parameter to be of type 'String'. " +
                    "The provided value is of type 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void StringShouldConvertToUpper()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "toUpper('One Two Three')",
                    "ONE TWO THREE"
                );
            }

            [Test]
            public static void StringShouldRoundtrip()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "toUpper(toLower('ONE TWO THREE'))",
                    "ONE TWO THREE"
                );
            }

        }

    }

}
