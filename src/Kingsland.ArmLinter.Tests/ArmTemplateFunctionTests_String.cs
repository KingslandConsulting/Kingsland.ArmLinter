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
                public static void NullStringsShouldThrow()
                {
                    var ex = Assert.Throws<ArgumentNullException>(
                        () =>
                        {
                            var actual = ArmTemplateFunctions.String.Concat((string[])null);
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
                            var actual = ArmTemplateFunctions.String.Concat(new string[] { });
                        }
                    );
                    var expectedMessage = "Concat requires at least one parameter.";
                    Assert.AreEqual(expectedMessage, ex.Message);
                }

                [Test]
                public static void OneStringShouldWork()
                {
                    var actual = ArmTemplateFunctions.String.Concat("hello");
                    var expected = "hello";
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void TwoStringsShouldWork()
                {
                    var actual = ArmTemplateFunctions.String.Concat("hello", "brave");
                    var expected = "hellobrave";
                    Assert.AreEqual(expected, actual);
                }

                [Test]
                public static void ManyStringsShouldWork()
                {
                    var actual = ArmTemplateFunctions.String.Concat("hello", "brave", "new", "world");
                    var expected = "hellobravenewworld";
                    Assert.AreEqual(expected, actual);
                }

            }

        }

    }

}
