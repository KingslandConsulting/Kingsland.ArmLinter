using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class ReplaceTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace()",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects exactly '3' parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('one', 'two', 'three', 'four')",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects exactly '3' parameters. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerOriginalStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace(100, 'bbb', 'ccc')",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its first parameter to be of type 'String'. The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerOldStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('aaa', 200, 'ccc')",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its second parameter to be of type 'String'. The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerNewStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('aaa', 'bbb', 300)",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its third parameter to be of type 'String'. The provided value is of type 'Integer'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayOriginalStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace(createArray(''), 'bbb', 'ccc')",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its first parameter to be of type 'String'. " +
                    $"The provided value is of type 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayOldStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('aaa', createArray(''), 'ccc')",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its second parameter to be of type 'String'. " +
                    $"The provided value is of type 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ArrayNewStringShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('aaa', 'bbb', createArray(''))",
                    typeof(ArgumentException),
                    $"The template language function 'replace' expects its third parameter to be of type 'String'. " +
                    $"The provided value is of type 'Array'. " +
                    $"Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ShouldReplaceWithEmptyString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "replace('123-123-1234', '-', '')",
                    "1231231234"
                );
            }

            [Test]
            public static void ShouldReplaceSingleMatch()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "replace('123-123-1234', '1234', 'xxxx')",
                    "123-123-xxxx"
                );
            }

            [Test]
            public static void ShouldReplaceMultipleMatches()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "replace('123-123-1234', '123', 'xxx')",
                    "xxx-xxx-xxx4"
                );
            }

            /// <summary>
            /// This might be a bug in the ARM Template API - executing this
            /// template seems to hang.
            ///
            /// We'll expect an exception to be thrown instead of an infinite
            /// loop.
            /// </summary>
            [Test]
            [Ignore("bug in arm api causes test to hang")]
            public static void ReplaceEmptyString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "replace('123-123-1234', '', '')",
                    typeof(InvalidOperationException),
                    null
                );
            }

        }

    }

}
