using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class SubstringTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring()",
                    typeof(ArgumentException),
                    "The template language function 'substring' expects at least two parameters and up to three parameters: " +
                    "a string, a start index and optionally a length. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one', 200, 300, 400)",
                    typeof(ArgumentException),
                    "The template language function 'substring' expects at least two parameters and up to three parameters: " +
                    "a string, a start index and optionally a length. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void EmptyStringShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "substring('', 0)",
                    ""
                );
            }

            [Test]
            public static void StringAndStartShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "substring('one two three', 4)",
                    "two three"
                );
            }

            [Test]
            public static void StringStartAndLengthShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "substring('one two three', 4, 3)",
                    "two"
                );
            }

            [Test]
            public static void ZeroLengthShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "substring('one two three', 4, 0)",
                    ""
                );
            }

            [Test]
            public static void NegativeStartIndexShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one two three', -1, 3)",
                    typeof(ArgumentException),
                    "Unable to evaluate the template language function 'substring'. " +
                    "The index parameter cannot be less than zero. " +
                    "The index: '-1'. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void StartIndexPastEndOfStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one two three', 99, 3)",
                    typeof(ArgumentException),
                    "Unable to evaluate the template language function 'substring'. " +
                    "The index parameter cannot be larger than the length of the string. " +
                    "The index parameter: '99', the length of the string parameter: '13'. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void NegativeLengthShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one two three', 3, -1)",
                    typeof(ArgumentException),
                    "Unable to evaluate the template language function 'substring'. " +
                    "The length parameter cannot be less than zero. " +
                    "The length parameter: '-1'. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void LengthExactlyToEndOfStringShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "substring('one', 1, 2)",
                    "ne"
                );
            }

            [Test]
            public static void LengthExactlyPastEndOfStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one', 1, 3)",
                    typeof(ArgumentException),
                    "Unable to evaluate the template language function 'substring'. " +
                    "The index and length parameters must refer to a location within the string. " +
                    "The index parameter: '1', the length parameter: '3', the length of the string parameter: '3'. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

            [Test]
            public static void LengthWayPastEndOfStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "substring('one', 1, 99)",
                    typeof(ArgumentException),
                    "Unable to evaluate the template language function 'substring'. " +
                    "The index and length parameters must refer to a location within the string. " +
                    "The index parameter: '1', the length parameter: '99', the length of the string parameter: '3'. " +
                    "Please see https://aka.ms/arm-template-expressions/#substring for usage details."
                );
            }

        }

    }

}
