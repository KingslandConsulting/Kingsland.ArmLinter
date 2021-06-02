using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class LastIndexOfTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "lastIndexOf()",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 0 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#lastIndexOf for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "lastIndexOf('one', 'two', 'three')",
                    typeof(ArgumentException),
                    $"Unable to evaluate template language function 'lastIndexOf': function requires 2 argument(s) while 3 were provided. " +
                    $"Please see https://aka.ms/arm-template-expressions/#lastIndexOf for usage details."
                );
            }

            [Test]
            public static void IntegerParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "lastIndexOf(100, 'bbb')",
                    typeof(ArgumentException),
                    $"The template language function 'lastIndexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'Integer' and 'String'. " +
                    $"Please see https://aka.ms/arm-template-expressions#lastIndexOf for usage details."
                );
            }

            [Test]
            public static void IntegerParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "lastIndexOf('aaa', 200)",
                    typeof(ArgumentException),
                    $"The template language function 'lastIndexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'String' and 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions#lastIndexOf for usage details."
                );
            }

            [Test]
            public static void ArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "lastIndexOf(createArray('aaa'), createArray('bbb'))",
                    typeof(ArgumentException),
                    $"The template language function 'lastIndexOf' expects its parameters to be of type string and string. " +
                    $"The provided value is of type 'Array' and 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions#lastIndexOf for usage details."
                );
            }

            [Test]
            public static void EmptyStringInEmptyStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "lastIndexOf('', '')",
                    0
                );
            }

            [Test]
            public static void EmptyStringInStringValueShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "lastIndexOf('abcdef', '')",
                    5
                );
            }

            [Test]
            public static void MatchAtEndOfStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "lastIndexOf('test', 't')",
                    3
                );
            }

            [Test]
            public static void CaseInsensitiveMatchShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "lastIndexOf('abcdef', 'CD')",
                    2
                );
            }

            [Test]
            public static void NoMatchShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "lastIndexOf('abcdef', 'z')",
                    -1
                );
            }

        }

    }

}
