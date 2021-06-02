using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class PadLeftTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft()",
                    typeof(InvalidOperationException),
                    $"Unable to evaluate template language function 'padLeft': function requires minimum two and maximum three arguments while '0' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft('one', 'two', 'three', 'four')",
                    typeof(InvalidOperationException),
                    $"Unable to evaluate template language function 'padLeft': function requires minimum two and maximum three arguments while '4' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void IntegerValueToPadShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "padLeft(100, 10, '0')",
                    "0000000100"
                );
            }

            [Test]
            public static void NegativeIntegerValueShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "padLeft(-100, 10, '0')",
                    "000000-100"
                );
            }

            [Test]
            public static void ArrayValueToPadShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft(createArray(''), 10, '0')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'padLeft': the first parameter is invalid. " +
                    $"The source string must be a String or integer type, while 'Array' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void IntegerPaddingCharShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft(10, 15, 30)",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'padLeft': the third parameter is invalid. " +
                    $"The padding character must be a string type, while 'Integer' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void EmptyPaddingShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft(10, 15, '')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'padLeft': the third parameter is invalid. " +
                    $"The padding character must be a single character, while '' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void MulticharPaddingShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft(10, 15, 'abc')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'padLeft': the third parameter is invalid. " +
                    $"The padding character must be a single character, while 'abc' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void DefaultPaddingCharShouldBeSpace()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "padLeft('123', 10)",
                    "       123"
                );
            }

            [Test]
            public static void PaddingCharShouldBeUsedWhenSpecified()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "padLeft('123', 10, '0')",
                    "0000000123"
                );
            }

            [Test]
            public static void ShortTotalLengthShouldNotPad()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "padLeft('123', 2, '0')",
                    "123"
                );
            }

            [Test]
            public static void NegativeTotalLengthShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft('123', -1, '0')",
                    typeof(InvalidOperationException),
                    $"Unable to evaluate template language function 'padLeft': the second parameter is invalid. " +
                    $"Total width must be a positive integer value and not greater than '16', while '-1' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

            [Test]
            public static void LargeTotalWidthShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "padLeft(100, 17)",
                    typeof(InvalidOperationException),
                    $"Unable to evaluate template language function 'padLeft': the second parameter is invalid. " +
                    $"Total width must be a positive integer value and not greater than '16', while '17' was provided. " +
                    $"The syntax is padLeft(string, totalWidth [, paddingChar]). " +
                    $"Please see https://aka.ms/arm-template-expressions/#padleft for usage details."
                );
            }

        }

    }

}
