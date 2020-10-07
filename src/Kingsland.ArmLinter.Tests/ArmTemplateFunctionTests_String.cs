using Kingsland.ArmLinter.Functions;
using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmTemplateFunctionTests
    {

        public static class StringTests
        {

            public static class ConcatTests
            {

                [Test]
                public static void NoParametersShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentException>(
                        () =>
                        {
                            var actual = ArmStringFunctions.Concat();
                        }
                    );
                    var expectedMessage = "Concat requires at least one parameter.";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void NullStringsShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentNullException>(
                        () =>
                        {
                            var actual = ArmStringFunctions.Concat(
                                null
                            );
                        }
                    );
                    var expectedMessage = "Value cannot be null. (Parameter 'args')";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void ZeroStringsShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentException>(
                        () =>
                        {
                            var actual = ArmStringFunctions.Concat(
                                Array.Empty<string>()
                            );
                        }
                    );
                    var expectedMessage = "Concat requires at least one parameter.";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void OneStringShouldWork()
                {
                    var actual = ArmStringFunctions.Concat(
                        "hello"
                    );
                    var expected = "hello";
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void TwoStringsShouldWork()
                {
                    var actual = ArmStringFunctions.Concat(
                        "hello", "brave"
                    );
                    var expected = "hellobrave";
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManyStringsShouldWork()
                {
                    var actual = ArmStringFunctions.Concat(
                        "hello", "brave", "new", "world"
                    );
                    var expected = "hellobravenewworld";
                    Assert.AreEqual(expected, actual);
                }

            }

        }

    }

}
