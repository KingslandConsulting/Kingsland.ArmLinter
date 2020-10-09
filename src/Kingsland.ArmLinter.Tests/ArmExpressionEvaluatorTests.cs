using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        private static void AssertEvaluatorTest(string expression, object expected)
        {
            var actual = ArmExpressionEvaluator.Evaluate(expression);
            Assert.AreEqual(expected, actual);
        }

        private static void AssertEvaluatorTestThrows(string expression, Type expectedType, string expectedMessage)
        {

            var ex = Assert.Throws(
                expectedType,
                () =>
                {
                    var actual = ArmExpressionEvaluator.Evaluate(expression);
                }
            );
            Assert.AreEqual(expectedMessage, ex.Message);
        }

    }

}
