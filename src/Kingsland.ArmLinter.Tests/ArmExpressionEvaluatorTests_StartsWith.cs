using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class StartsWithTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'startsWith': function requires 2 argument(s) while 0 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#startsWith for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith('one', 'two', 'three')",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'startsWith': function requires 2 argument(s) while 3 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#startsWith for usage details."
                );
            }

            [Test]
            public static void IntegersShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith(100, 200)",
                    typeof(ArgumentException),
                    "The template language function 'startsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'Integer' and 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#startsWith for usage details."
                );
            }

            [Test]
            public static void ArrayForParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith(createArray('one'), 'two')",
                    typeof(ArgumentException),
                    "The template language function 'startsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'Array' and 'String'. " +
                    "Please see https://aka.ms/arm-template-expressions#startsWith for usage details."
                );
            }

            [Test]
            public static void ArrayForParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith('one', createArray('two'))",
                    typeof(ArgumentException),
                    "The template language function 'startsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'String' and 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions#startsWith for usage details."
                );
            }

            [Test]
            public static void StringAndIntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "startsWith('one', 100)",
                    typeof(ArgumentException),
                    "The template language function 'startsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'String' and 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#startsWith for usage details."
                );
            }

            [Test]
            public static void MatchingCaseShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "startsWith('abcdef', 'ab')",
                    true
                );
            }

            [Test]
            public static void CaseSensitiveMatchShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "startsWith('abcdef', 'A')",
                    true
                );
            }

            [Test]
            public static void NoMatchShouldReturnFalse()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "startsWith('abcdef', 'e')",
                    false
                );
            }

            [Test]
            public static void EmptyStringStartsWithEmptyStringShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "startsWith('', '')",
                    true
                );
            }

            [Test]
            public static void StringValueStartsWithEmptyStringShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "startsWith('abcdef', '')",
                    true
                );
            }

        }

    }

}
