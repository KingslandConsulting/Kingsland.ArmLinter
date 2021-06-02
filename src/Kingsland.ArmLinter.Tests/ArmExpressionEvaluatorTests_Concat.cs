using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class ConcatTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "concat()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'concat'. " +
                    "At least one parameter should be provided. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            #region String Tests

            [Test]
            public static void OneEmptyStringShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('')",
                    ""
                );
            }

            [Test]
            public static void MultipleEmptyStringsShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('', '', '')",
                    ""
                );
            }

            [Test]
            public static void OneStringShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('hello')",
                    "hello"
                );
            }

            [Test]
            public static void TwoStringsShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('hello', 'brave')",
                    "hellobrave"
                );
            }

            [Test]
            public static void ManyStringsShouldWork_1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('hello', 'brave', 'new', 'world')",
                    "hellobravenewworld"
                );
            }

            [Test]
            public static void ManyStringsShouldWork_2()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('hello', '', 'brave', '', 'new', '', 'world')",
                    "hellobravenewworld"
                );
            }

            [Test]
            public static void NestedConcatShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat('hello', concat('brave', 'new'), 'world')",
                    "hellobravenewworld"
                );
            }

            [Test]
            public static void CompoundStringTest1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(toLower('ONE'), '-', toUpper('two'), '-', base64('three'))",
                    "one-TWO-dGhyZWU="
                );
            }

            #endregion

            #region Integer Tests

            [Test]
            public static void OneIntegerShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(100)",
                    "100"
                );
            }

            [Test]
            public static void TwoIntegersShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(100, 200)",
                    "100200"
                );
            }

            #endregion

            #region Array Tests

            [Test]
            public static void OneEmptyArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(intersection(createArray('aaa'), createArray('bbb')))",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void MultipleEmptyArraysShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(intersection(createArray('aaa'), createArray('bbb')), intersection(createArray('ccc'), createArray('ddd')), intersection(createArray('eee'), createArray('fff')))",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void OneSingleStringItemtArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello'))",
                    new object[] { "hello" }
                );
            }

            [Test]
            public static void TwoSingleStringItemArraysShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello'), createArray('brave'))",
                    new object[] { "hello", "brave" }
                );
            }

            [Test]
            public static void FourSingleStringItemArraysShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(concat(createArray('hello'), createArray('brave'), createArray('new'), createArray('world')))",
                    new object[] { "hello", "brave", "new", "world" }
                );
            }

            [Test]
            public static void OneMultipleStringItemArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello', 'brave'))",
                    new object[] { "hello", "brave" }
                );
            }

            [Test]
            public static void TwoMultipleStringItemArraysShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello', 'brave'), createArray('new', 'world'))",
                    new object[] { "hello", "brave", "new", "world" }
                );
            }

            [Test]
            public static void EmptyArraysShouldBeOmitted()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello'), intersection(createArray('aaa'), createArray('bbb')), createArray('brave'), intersection(createArray('ccc'), createArray('ddd')), createArray('new'), intersection(createArray('eee'), createArray('fff')), createArray('world'))",
                    new object[] { "hello", "brave", "new", "world" }
                );
            }

            [Test]
            public static void MixedItemArraysShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(createArray('hello', 100), createArray('new', 200))",
                    new object[] { "hello", 100, "new", 200 }
                );
            }

            #endregion

            [Test]
            public static void MixedPrimitiveTypesShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "concat(100, 'aaa', 200)",
                    "100aaa200"
                );
            }

            [Test]
            public static void MixedArraysAndPrimitiveShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "concat(createArray('hello', 'brave'), 'new', 'world')",
                    typeof(ArgumentException),
                    "The provided parameters for language function 'concat' are invalid. " +
                    "Either all or none of the parameters must be an array. " +
                    "Please see https://aka.ms/arm-template-expressions/#concat for usage details."
                );
            }

        }

    }

}
