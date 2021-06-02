using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class TakeTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "take()",
                    typeof(ArgumentException),
                    "The template language function 'take' expects exactly two parameters: " +
                    "the collection to take the objects from as the first parameter " +
                    "and the count of objects to take as the second parameter. " +
                    "The function was invoked with '0' parameter(s). " +
                    "Please see https://aka.ms/arm-template-expressions#take for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "take('one', 'two', 'three')",
                    typeof(ArgumentException),
                    "The template language function 'take' expects exactly two parameters: " +
                    "the collection to take the objects from as the first parameter and " +
                    "the count of objects to take as the second parameter. " +
                    "The function was invoked with '3' parameter(s). " +
                    "Please see https://aka.ms/arm-template-expressions#take for usage details."
                );
            }

            [Test]
            public static void IntegerForParameter1ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "take(100, 2)",
                    typeof(ArgumentException),
                    "The template language function 'take' expects its first parameter 'collection' to be an array or a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#take for usage details."
                );
            }

            [Test]
            public static void StringForParameter2ShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "take('one two three', 'aaa')",
                    typeof(ArgumentException),
                    "The template language function 'take' expects its second parameter 'count' to be an integer. " +
                    "The provided value is of type 'String'. " +
                    "Please see https://aka.ms/arm-template-expressions#take for usage details."
                );
            }

            //[Test]
            //public static void ShouldTakeFromEmptyArray()
            //{
            //    // "test-output": {
            //    //   "type": "string",
            //    //   "value": "[take(createArray(''), 2)]"
            //    // },
            //    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
            //        "take(createArray(''), 2)",
            //        Array.Empty<object>()
            //    );
            //}

            [Test]
            public static void ShouldTakeFromEmptyString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('', 5)",
                    ""
                );
            }

            [Test]
            public static void ShouldTakeCharacters()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('one two three', 2)",
                    "on"
                );
            }

            [Test]
            public static void ShouldTakeWithZeroLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('one two three', 0)",
                    ""
                );
            }


            [Test]
            public static void ShouldTakeWithNegativeLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('one two three', -100)",
                    ""
                );
            }

            [Test]
            public static void ShouldTakeWithExactStringLength()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('one two three', 13)",
                    "one two three"
                );
            }

            [Test]
            public static void ShouldTakeIfPastEndOfString()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "take('one two three', 100)",
                    "one two three"
                );
            }

        }

    }

}
