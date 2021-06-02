using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class EndsWithTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 0 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#endsWith for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith('one', 'two', 'three')",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'endsWith': function requires 2 argument(s) while 3 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#endsWith for usage details."
                );
            }

            [Test]
            public static void IntegersShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith(100, 200)",
                    typeof(ArgumentException),
                    "The template language function 'endsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'Integer' and 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#endsWith for usage details."
                );
            }

            [Test]
            public static void ArrayForParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith(createArray('one'), 'two')",
                    typeof(ArgumentException),
                    "The template language function 'endsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'Array' and 'String'. " +
                    "Please see https://aka.ms/arm-template-expressions#endsWith for usage details."
                );
            }

            [Test]
            public static void ArrayForParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith('one', createArray('two'))",
                    typeof(ArgumentException),
                    "The template language function 'endsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'String' and 'Array'. " +
                    "Please see https://aka.ms/arm-template-expressions#endsWith for usage details."
                );
            }

            [Test]
            public static void StringAndIntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "endsWith('one', 100)",
                    typeof(ArgumentException),
                    "The template language function 'endsWith' expects its parameters to be of type string and string. " +
                    "The provided value is of type 'String' and 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#endsWith for usage details."
                );
            }

            [Test]
            public static void MatchingCaseShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "endsWith('abcdef', 'ef')",
                    true
                );
            }

            [Test]
            public static void CaseSensitiveMatchShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "endsWith('abcdef', 'F')",
                    true
                );
            }

            [Test]
            public static void NoMatchShouldReturnFalse()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "endsWith('abcdef', 'e')",
                    false
                );
            }

            [Test]
            public static void EmptyStringEndsWithEmptyStringShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "endsWith('', '')",
                    true
                );
            }

            [Test]
            public static void StringValueEndsWithEmptyStringShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "endsWith('abcdef', '')",
                    true
                );
            }

        }

    }

}
