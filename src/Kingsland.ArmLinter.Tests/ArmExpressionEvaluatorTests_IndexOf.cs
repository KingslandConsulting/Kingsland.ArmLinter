using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class IndexOfTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "indexOf()",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 0 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#indexOf for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "indexOf('one', 'two', 'three')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'indexOf': function requires 2 argument(s) while 3 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#indexOf for usage details."
                );
            }

            [Test]
            public static void IntegerParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "indexOf(100, 'bbb')",
                    typeof(ArgumentException),
                    $"The template language function 'indexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'Integer' and 'String'. " +
                    $"Please see https://aka.ms/arm-template-expressions#indexOf for usage details."
                );
            }

            [Test]
            public static void IntegerParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "indexOf('aaa', 200)",
                    typeof(ArgumentException ),
                    $"The template language function 'indexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'String' and 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions#indexOf for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "indexOf(createArray('aaa'), createArray('bbb'))",
                    typeof(ArgumentException),
                    $"The template language function 'indexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'Array' and 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions#indexOf for usage details."
                );
            }

            [Test]
            public static void EmptyStringInEmptyStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "indexOf('', '')",
                    0
                );
            }

            [Test]
            public static void EmptyStringInStringValueShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "indexOf('abcdef', '')",
                    0
                );
            }

            [Test]
            public static void MatchAtStartOfStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "indexOf('test', 't')",
                    0
                );
            }

            [Test]
            public static void CaseInsensitiveMatchShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "indexOf('abcdef', 'CD')",
                    2
                );
            }

            [Test]
            public static void NoMatchShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "indexOf('abcdef', 'z')",
                    -1
                );
            }

        }

    }

}
