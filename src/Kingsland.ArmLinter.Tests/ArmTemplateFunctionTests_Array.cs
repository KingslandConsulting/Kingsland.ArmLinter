using Kingsland.ArmLinter.Functions;
using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmTemplateFunctionTests
    {

        public static class ArrayTests
        {

            public static class ConcatTests
            {

                [Test]
                public static void NoParametersShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentException>(
                        () =>
                        {
                            var actual = ArmArrayFunctions.Concat();
                        }
                    );
                    var expectedMessage = "Concat requires at least one parameter.";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void NullArrayShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentNullException>(
                        () =>
                        {
                            var actual = ArmArrayFunctions.Concat(
                                null
                            );
                        }
                    );
                    var expectedMessage = "Value cannot be null. (Parameter 'args')";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void ZeroSubArraysShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentException>(
                        () =>
                        {
                            var actual = ArmArrayFunctions.Concat(
                                Array.Empty<object[]>())
                            ;
                        }
                    );
                    var expectedMessage = "Concat requires at least one parameter.";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void OneSingleItemSubArrayShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "hello" }
                    );
                    var expected = new object[] { "hello" };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void TwoSingleItemSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "hello" },
                        new object[] { "brave" }
                    );
                    var expected = new object[] { "hello", "brave" };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManySingleItemSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "hello" },
                        new object[] { "brave" },
                        new object[] { "new" },
                        new object[] { "world" }
                    );
                    var expected = new object[] { "hello", "brave", "new", "world" };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void OneMultiItemSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "1-1", "1-2", "1-3" }
                    );
                    var expected = new object[] {
                        "1-1", "1-2", "1-3"
                    };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void TwoMultiItemSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "1-1", "1-2", "1-3" },
                        new object[] { "2-1", "2-2", "2-3" }
                    );
                    var expected = new object[] {
                        "1-1", "1-2", "1-3",
                        "2-1", "2-2", "2-3"
                    };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManyMultiItemStringSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "1-1", "1-2", "1-3" },
                        new object[] { "2-1", "2-2", "2-3" },
                        new object[] { "3-1", "3-2", "3-3" },
                        new object[] { "4-1", "4-2", "4-3" }
                    );
                    var expected = new object[] {
                        "1-1", "1-2", "1-3",
                        "2-1", "2-2", "2-3",
                        "3-1", "3-2", "3-3",
                        "4-1", "4-2", "4-3"
                    };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManyMultiItemIntSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { 11, 12, 13 },
                        new object[] { 21, 22, 23 },
                        new object[] { 31, 32, 33 },
                        new object[] { 41, 42, 43 }
                    );
                    var expected = new object[] {
                        11, 12, 13,
                        21, 22, 23,
                        31, 32, 33,
                        41, 42, 43
                    };
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManyMultiItemMixedSubArraysShouldWork()
                {
                    var actual = ArmArrayFunctions.Concat(
                        new object[] { "1-1", 12, "1-3" },
                        new object[] { 21, "2-2", 23 },
                        new object[] { "3-1", 32, "3-3" },
                        new object[] { 41, "4-2", 43 }
                    );
                    var expected = new object[] {
                        "1-1", 12, "1-3",
                        21, "2-2", 23,
                        "3-1", 32, "3-3",
                        41, "4-2", 43
                    };
                    Assert.AreEqual(expected, actual);
                }

            }

        }

    }

}
