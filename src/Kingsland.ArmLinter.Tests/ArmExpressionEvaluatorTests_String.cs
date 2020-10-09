using NUnit.Framework;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        public static class StringTests
        {

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

            public static class EndsWithTests
            {

                [Test]
                public static void MatchingCaseShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'ef')",
                        true
                    );
                }

                [Test]
                public static void CaseSensitiveMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'F')",
                        true
                    );
                }

                [Test]
                public static void NoMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "endsWith('abcdef', 'e')",
                        false
                    );
                }

            }

            public static class FirstTests
            {

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "first('One Two Three')",
                        "O"
                    );
                }

            }

            public static class FormatTests
            {

                [Test]
                public static void StringShouldFormat()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "format('{0}, {1}. Formatted number: {2:N0}', 'Hello', 'User', 8175133)",
                        "Hello, User. Formatted number: 8,175,133"
                    );
                }

            }

            public static class IndexOfTests
            {

                [Test]
                public static void EmptyStringShouldReturn_1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringShouldReturn_2()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', '')",
                        0
                    );
                }

                [Test]
                public static void MatchAtStartShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('test', 't')",
                        0
                    );
                }

                [Test]
                public static void MatchShouldBeCaseInsensitive()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'CD')",
                        2
                    );
                }

                [Test]
                public static void NotFoundSHouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LastTests
            {

                [Test]
                public static void EmptyStringShouldReturnEmptyString()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('')",
                        ""
                    );
                }

                [Test]
                public static void NonEmptyStringShouldReturnFirstChar()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "last('One Two Three')",
                        "e"
                    );
                }

            }

            public static class LastIndexOfTests
            {

                [Test]
                public static void EmptyStringShouldReturn_1()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('', '')",
                        0
                    );
                }

                [Test]
                public static void EmptyStringShouldReturn_2()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', '')",
                        5
                    );
                }

                [Test]
                public static void MatchAtEndShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('test', 't')",
                        3
                    );
                }

                [Test]
                public static void MatchShouldBeCaseInsensitive()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "lastIndexOf('abcdef', 'AB')",
                        0
                    );
                }

                [Test]
                public static void NotFoundShouldReturn()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "indexOf('abcdef', 'z')",
                        -1
                    );
                }

            }

            public static class LengthTests
            {

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
                        "length('One Two Three')",
                        13
                    );
                }

            }

            public static class PadLeftTests
            {

                [Test]
                public static void SampleStringShouldPadLeft()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10, '0')",
                        "0000000123"
                    );
                }

                [Test]
                public static void ShortStringShouldNotAppendPadding()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 2, '0')",
                        "123"
                    );
                }

                [Test]
                public static void DefaultPaddingCharacterShouldBeSpace()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "padLeft('123', 10)",
                        "       123"
                    );
                }

            }

            public static class ReplaceTests
            {

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

            }

            public static class SkipTests
            {

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

            public static class SplitTests
            {

                [Test]
                public static void ShouldSplitOneDelimiter()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two,three', ',')",
                        new string[] { "one", "two", "three" }
                    );
                }

                [Test]
                public static void ShouldSplitMultipleDelimiter()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "split('one,two;three', createArray(',', ';'))",
                        new string[] { "one", "two", "three" }
                    );
                }

            }

            public static class StartsWithTests
            {

                [Test]
                public static void MatchingCaseShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'ab')",
                        true
                    );
                }

                [Test]
                public static void CaseSensitiveMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'A')",
                        true
                    );
                }

                [Test]
                public static void NoMatchShouldWork()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "startsWith('abcdef', 'e')",
                        false
                    );
                }

            }

            public static class SubstringTests
            {

                [Test]
                public static void ShouldReturnSubstring()
                {
                    ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                        "substring('one two three', 4, 3)",
                        "two"
                    );
                }

            }

            public static class TakeTests
            {

                [Test]
                public static void ShouldTakeEmptyString()
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

            public static class TrimTests
            {

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

}
