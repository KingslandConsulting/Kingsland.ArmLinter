using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class IntersectionTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "intersection()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'intersection'. " +
                    "At least two parameter should be provided. " +
                    "Please see https://aka.ms/arm-template-expressions for usage details."
                );
            }

            [Test]
            public static void IntegersShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "intersection(100, 200)",
                    typeof(InvalidOperationException),
                    "The template language function 'intersection' expects either a comma separated list of arrays or a comma separated list of objects as its parameters. " +
                    "Please see https://aka.ms/arm-template-expressions#intersection for usage details."
                );
            }

            [Test]
            public static void StringsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "intersection('aaa', 'bbb')",
                    typeof(InvalidOperationException),
                    "The template language function 'intersection' expects either a comma separated list of arrays or a comma separated list of objects as its parameters. " +
                    "Please see https://aka.ms/arm-template-expressions#intersection for usage details."
                );
            }

            [Test]
            public static void MixedParameterTypesShouldThrow_1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "intersection(100, createArray('bbb'))",
                    typeof(InvalidOperationException),
                    "Template language function 'intersection' expects parameters of the same type, but found multiple types."
                );
            }

            [Test]
            public static void MixedParameterTypesShouldThrow_2()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "intersection(createArray('aaa'), 200)",
                    typeof(InvalidOperationException),
                    "Template language function 'intersection' expects parameters of the same type, but found multiple types."
                );
            }

            [Test]
            public static void IntegerArraysShouldWork_1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray(100), createArray(200))",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void IntegerArraysShouldWork_2()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray(100, 200, 300), createArray(400, 300, 500))",
                    new object[] { 300 }
                );
            }

            [Test]
            public static void IntegerArraysShouldWork_3()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray(100, 200), createArray(200, 100))",
                    new object[] { 100, 200 }
                );
            }

            [Test]
            public static void StringArraysShouldWork_1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa'), createArray('bbb'))",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void StringArraysShouldWork_2()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa', 'bbb', 'ccc'), createArray('ddd', 'ccc', 'eee'))",
                    new object[] { "ccc" }
                );
            }

            [Test]
            public static void StringArraysShouldWork_3()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa', 'bbb'), createArray('bbb', 'aaa'))",
                    new object[] { "aaa", "bbb" }
                );
            }

            [Test]
            public static void MixedValueArraysShouldWork_1()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa', 100), createArray(200, 'bbb'))",
                    Array.Empty<object>()
                );
            }

            [Test]
            public static void MixedValueArraysShouldWork_2()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa', 100), createArray(100, 'bbb'))",
                    new object[] { 100 }
                );
            }

            [Test]
            public static void MixedValueArraysShouldWork_3()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "intersection(createArray('aaa', 100, 'ccc'), createArray(100, 'aaa', 'ddd'))",
                    new object[] { "aaa", 100 }
                );
            }

        }

    }

}
