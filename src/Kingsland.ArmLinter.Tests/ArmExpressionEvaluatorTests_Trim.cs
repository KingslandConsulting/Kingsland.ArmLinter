using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class TrimTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "trim()",
                    typeof(ArgumentException),
                    "The template language function 'trim' expects exactly '1' parameters. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "trim('one', 'two', 'three')",
                    typeof(ArgumentException),
                    "The template language function 'trim' expects exactly '1' parameters. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegerForParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "trim(100)",
                    typeof(ArgumentException),
                    "The template language function 'trim' expects its first parameter to be of type 'String'. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void ShouldTrimEmptyString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "trim('')",
                    ""
                );
            }

            [Test]
            public static void ShouldTrimString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "trim('    one two three   ')",
                    "one two three"
                );
            }

        }

    }

}
