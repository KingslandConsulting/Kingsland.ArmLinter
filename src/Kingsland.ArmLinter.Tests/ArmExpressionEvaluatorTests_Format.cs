using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class FormatTests
        {


            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "format()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'format'. " +
                    "At least one parameter should be provided. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void OneStringArgumentShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('aaa')",
                    "aaa"
                );
            }

            [Test]
            public static void OneIntegerArgumentShouldthrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "format(100)",
                    typeof(ArgumentException),
                    "Unable to evaluate language function 'format': " +
                    "the first argument must be a string literal, and the type of other arguments must be one of 'Array, Boolean, Date, Float, Guid, Integer, Null, Object, String, TimeSpan, Undefined, Uri'. " +
                    "Please see https://aka.ms/arm-template-expressions/#format for usage details."
                );
            }

            [Test]
            public static void StringArgShouldFormat()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('...{0}...', 'aaa')",
                    "...aaa..."
                );
            }

            [Test]
            public static void IntegerArgShouldFormat()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('...{0}...', 100)",
                    "...100..."
                );
            }


            [Test]
            public static void LargeIntegerArgShouldFormat()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('...{0}...', 100000)",
                    "...100000..."
                );
            }

            [Test]
            public static void FormatStringShouldApply()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('...{0:N0}...', 100000)",
                    "...100,000..."
                );
            }

            [Test]
            [Ignore("boolean not implemented in lexer yet")]
            public static void BooleanArgShouldFormat()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('...{0}...', true)",
                    "...true..."
                );
            }

            [Test]
            public static void MixedArgsShouldFormat()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "format('{0}, {1}. Formatted number: {2:N0}', 'Hello', 'User', 8175133)",
                    "Hello, User. Formatted number: 8,175,133"
                );
            }

        }

    }

}
