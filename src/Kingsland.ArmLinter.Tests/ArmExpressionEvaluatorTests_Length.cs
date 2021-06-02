using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class LengthTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "length()",
                    typeof(ArgumentException),
                    "The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. " +
                    "The function was invoked with '0' parameters. " +
                    "Please see https://aka.ms/arm-template-expressions/#length for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "length('one', 'two')",
                    typeof(ArgumentException),
                    "The template language function 'length' expects exactly one parameter: an array, object, or a string the length of which is returned. " +
                    "The function was invoked with '2' parameters. " +
                    "Please see https://aka.ms/arm-template-expressions/#length for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "length(100)",
                    typeof(ArgumentException),
                    "The template language function 'length' expects its parameter to be an array, object, or a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions/#length for usage details."
                );
            }

            [Test]
            public static void EmptyArrayShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "length(intersection(createArray('aaa'), createArray('bbb')))",
                    0
                );
            }

            [Test]
            public static void ArrayWithSingleValusShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "length(createArray('one'))",
                    1
                );
            }

            public static void ArrayWithMultipleValuesShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "length(createArray('one', 'two'))",
                    2
                );
            }

            [Test]
            public static void EmptyStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "length('')",
                    0
                );
            }

            [Test]
            public static void NonEmptyStringShouldReturn()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "length('abc')",
                    3
                );
            }

        }

    }

}
