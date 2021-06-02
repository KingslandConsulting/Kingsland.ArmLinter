using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class SplitTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "split()",
                    typeof(ArgumentException),
                    $"The template language function 'split' expects exactly '2' parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "split('one', 'two', 'three')",
                    typeof(ArgumentException),
                    $"The template language function 'split' expects exactly '2' parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerInputStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "split(100, ',')",
                    typeof(ArgumentException),
                    $"The template language function 'split' expects its first parameter to be of type 'String'. " +
                    $"The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerDelimiterShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "split('one,two,three', 200)",
                    typeof(ArgumentException),
                    $"The template language function 'split' expects its second parameter to be of type 'String or Array of Strings'. " +
                    $"The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayInputStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "split(createArray(''), ',')",
                    typeof(ArgumentException),
                    $"The template language function 'split' expects its first parameter to be of type 'String'. " +
                    $"The provided value is of type 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayDelimiterShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('one,two;three', createArray(',', ';'))",
                    new string[] { "one", "two", "three" }
                );
            }

            [Test]
            public static void ShouldSplitStringDelimiter()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('one,two,three', ',')",
                    new string[] { "one", "two", "three" }
                );
            }

            [Test]
            public static void ShouldSplitEmptyStringAndEmptyDelimiter()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('', '')",
                    new string[] { "" }
                );
            }

            [Test]
            public static void ShouldSplitStringAndEmptyStringArrayDelimiter()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('one,two,three', createArray(''))",
                    new string[] { "one,two,three" }
                );
            }

            [Test]
            public static void ShouldSplitStringAndEmptyArrayDelimiter()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('one,two,three', intersection(createArray('aaa'), createArray('bbb')))",
                    new string[] { "one,two,three" }
                );
            }

            [Test]
            public static void EmptyResultStringsShouldBeIncluded()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "split('one,,,two;;;three', createArray(',', ';'))",
                    new string[] { "one", "", "", "two", "", "", "three" }
                );
            }

        }

    }

}
