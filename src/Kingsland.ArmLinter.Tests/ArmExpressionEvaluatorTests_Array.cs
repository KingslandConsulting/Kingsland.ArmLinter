using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        public static class ArrayTests
        {

            public static class CreateArrayTests
            {

                [Test]
                public static void ShouldCreateArrayFromEmptyValues()
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

            }

        }

    }

}
