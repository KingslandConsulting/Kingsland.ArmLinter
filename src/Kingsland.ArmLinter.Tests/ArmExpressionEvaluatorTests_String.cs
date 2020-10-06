using NUnit.Framework;

namespace Kingsland.ArmLinter.Tests
{

    public static class ArmExpressionEvaluatorTests
    {

        public static class StringTests
        {

            public static class ConcatTests
            {

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
                public static void ManyStringsShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat('hello', 'brave', 'new', 'world')",
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
                public static void CompoundTest1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "concat(toLower('ONE'), '-', toUpper('two'), '-', base64('three'))",
                        "one-TWO-dGhyZWU="
                    );
                }

            }

            public static class Base64Tests
            {

                [Test]
                public static void SampleStringShouldEncodeToBase64()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64('one, two, three')",
                        "b25lLCB0d28sIHRocmVl"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64ToString(base64('one, two, three'))",
                        "one, two, three"
                    );
                }

            }

            public static class Base64ToStringTests
            {

                [Test]
                public static void SampleBase64StringShouldDecodeToOriginalString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64ToString('b25lLCB0d28sIHRocmVl')",
                        "one, two, three"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "base64(base64ToString('b25lLCB0d28sIHRocmVl'))",
                        "b25lLCB0d28sIHRocmVl"
                    );
                }

            }

            public static class ToLowerTests
            {

                [Test]
                public static void SampleStringShouldConvertToLower()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toLower('One Two Three')",
                        "one two three"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toLower(toUpper('one two three'))",
                        "one two three"
                    );
                }

            }

            public static class ToUpperTests
            {

                [Test]
                public static void SampleStringShouldConvertToUpper()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toUpper('One Two Three')",
                        "ONE TWO THREE"
                    );
                }

                [Test]
                public static void SampleStringShouldRoundtrip()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "toUpper(toLower('ONE TWO THREE'))",
                        "ONE TWO THREE"
                    );
                }

            }

        }

        private static void AssertEvaluatorTest(string expression, object expected)
        {
            var actual = ArmExpressionEvaluator.Evaluate(expression);
            Assert.AreEqual(expected, actual);
        }

    }

}
