using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class SkipTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                var typoWord = "skip"; // should be "take"
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "skip()",
                    typeof(ArgumentException),
                    // mirror the typo in azure arm api - "skip" verb should be "take"
                    $"The template language function 'skip' expects exactly two parameters: " +
                    $"the collection to {typoWord} the objects from as the first parameter " +
                    $"and the count of objects to {typoWord} as the second parameter. " +
                    $"The function was invoked with '0' parameter(s). " +
                    $"Please see https://aka.ms/arm-template-expressions#skip for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                var typoWord = "skip"; // should be "take"
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "skip('one', 'two', 'three')",
                    typeof(ArgumentException),
                    // mirror the typo in azure arm api - "skip" verb should be "take"
                    $"The template language function 'skip' expects exactly two parameters: " +
                    $"the collection to {typoWord} the objects from as the first parameter and " +
                    $"the count of objects to {typoWord} as the second parameter. " +
                    $"The function was invoked with '3' parameter(s). " +
                    $"Please see https://aka.ms/arm-template-expressions#skip for usage details."
                );
            }

            [Test]
            public static void IntegerForParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "skip(100, 2)",
                    typeof(ArgumentException),
                    "The template language function 'skip' expects its first parameter 'collection' to be an array or a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#skip for usage details."
                );
            }

            [Test]
            public static void StringForParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "skip('one two three', 'aaa')",
                    typeof(ArgumentException),
                    "The template language function 'skip' expects its second parameter 'count' to be an integer. " +
                    "The provided value is of type 'String'. " +
                    "Please see https://aka.ms/arm-template-expressions#skip for usage details."
                );
            }

            //[Test]
            //public static void ShouldTakeFromEmptyArray()
            //{
            //    // "test-output": {
            //    //   "type": "string",
            //    //   "value": "[skip(createArray(''), 2)]"
            //    // },
            //    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
            //        "skip(createArray(''), 2)",
            //        Array.Empty<object>()
            //    );
            //}

            [Test]
            public static void ShouldSkipEmptyString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('', 5)",
                    ""
                );
            }

            [Test]
            public static void ShouldSkipCharacters()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('one two three', 4)",
                    "two three"
                );
            }

            [Test]
            public static void ShouldSkipWithZeroLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('one two three', 0)",
                    "one two three"
                );
            }

            [Test]
            public static void ShouldSkipWithNegativeLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('one two three', -100)",
                    "one two three"
                );
            }

            [Test]
            public static void ShouldSkipWithExactStringLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('one two three', 13)",
                    ""
                );
            }

            [Test]
            public static void ShouldSkipIfPastEndOfString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "skip('one two three', 100)",
                    ""
                );
            }

        }

    }

}
