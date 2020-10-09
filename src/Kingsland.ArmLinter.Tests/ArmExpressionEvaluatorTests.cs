using NUnit.Framework;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        private static void AssertEvaluatorTest(string expression, object expected)
        {
            var actual = ArmExpressionEvaluator.Evaluate(expression);
            Assert.AreEqual(expected, actual);
        }

    }

}
