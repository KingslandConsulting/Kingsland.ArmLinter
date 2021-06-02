using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class CreateArrayTests
        {

            [Test]
            public static void InvokingWithNoArgumentsShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "createArray()",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void ShouldCreateArrayFromStringValues()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "createArray('a', 'b', 'c')",
                    new object[] { "a", "b", "c" }
                );
            }

            [Test]
            public static void ShouldCreateArrayFromIntValues()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "createArray(1, 2, 3)",
                    new object[] { 1, 2, 3 }
                );
            }

            [Test]
            public static void ShouldCreateArrayFromMixedValues()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "createArray(1, 'bbb', 3)",
                    new object[] { 1, "bbb", 3 }
                );
            }

        }

    }

}
