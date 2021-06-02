using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class EmptyTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "empty()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'empty': function requires 1 argument(s) while 0 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#empty for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "empty('one', 'two')",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'empty': function requires 1 argument(s) while 2 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#empty for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "empty(100)",
                    typeof(ArgumentException),
                    "The template function 'empty' expects its parameter to be an object, an array, or a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#empty for usage details."
                );
            }

            [Test]
            public static void EmptyArrayShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "empty(intersection(createArray('aaa'), createArray('bbb')))",
                    true
                );
            }

            [Test]
            public static void ArrayWithValuesShouldReturnFalse()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "empty(createArray('one'))",
                    false
                );
            }

            [Test]
            public static void EmptyStringShouldReturnTrue()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "empty('')",
                    true
                );
            }

            [Test]
            public static void NonEmptyStringShouldReturnFalse()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "empty('abc')",
                    false
                );
            }

        }

    }

}
