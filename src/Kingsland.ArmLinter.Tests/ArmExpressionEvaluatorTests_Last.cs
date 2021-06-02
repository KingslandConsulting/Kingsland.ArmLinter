using NUnit.Framework;
using System;

namespace Kingsland.ArmLinter.Tests
{

    public static partial class ArmExpressionEvaluatorTests
    {

        [TestFixture]
        [Parallelizable(ParallelScope.All)]
        public static class LastTests
        {

            [Test]
            public static void NoArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "last()",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'last': function requires 1 argument(s) while 0 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#last for usage details."
                );
            }

            [Test]
            public static void TooManyArgumentsShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "last('one', 'two')",
                    typeof(ArgumentException),
                    "Unable to evaluate template language function 'last': function requires 1 argument(s) while 2 were provided. " +
                    "Please see https://aka.ms/arm-template-expressions/#last for usage details."
                );
            }

            [Test]
            public static void IntegerShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "last(100)",
                    typeof(ArgumentException),
                    "The template language function 'last' expects its parameter be an array or a string. " +
                    "The provided value is of type 'Integer'. " +
                    "Please see https://aka.ms/arm-template-expressions#last for usage details."
                );
            }

            /// <summary>
            /// This is possibly a bug in the ARM Template API.
            /// </summary>
            /// <remarks>
            /// -- request --
            /// PUT https://management.azure.com/subscriptions/d160aa00-8080-4d57-aff9-839ad231bbb4/resourcegroups/my-resourcegroup/providers/Microsoft.Resources/deployments/my-deployment?api-version=2020-10-01
            /// {
            ///   "properties": {
            ///     "template": {
            ///       "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
            ///       "contentVersion": "1.0.0.0",
            ///       "resources": [],
            ///       "outputs": {
            ///         "result": {
            ///           "type": "Int",
            ///           "value": "[last(intersection(createArray('aaa'), createArray('bbb')))]"
            ///         }
            ///       }
            ///     },
            ///     "mode": "Incremental"
            ///   }
            /// }
            /// -- response --
            /// (deployment starts ok, but fails with an error)
            /// HTTP/1.1 200 OK
            /// {
            ///   "id": "/subscriptions/d160aa00-8080-4d57-aff9-839ad231bbb4/resourceGroups/my-resourcegroup/providers/Microsoft.Resources/deployments/my-deployment-12345",
            ///   "name": "my-deployment-12345",
            ///   "type":"Microsoft.Resources/deployments",
            ///   "properties": {
            ///     "templateHash": "5244381047673507689",
            ///     "mode": "Incremental",
            ///     "provisioningState": "Failed",
            ///     "timestamp": "2021-04-20T14:38:10.1333042Z",
            ///     "duration": "PT3.2183141S",
            ///     "correlationId": "ee36f25c-d008-4011-9f77-17c613a46284",
            ///     "providers": [],
            ///     "dependencies": [],
            ///     "error": {
            ///       "code": "DeploymentOutputEvaluationFailed",
            ///       "message": "Unable to evaluate template outputs: 'result'. Please see error details and deployment operations. Please see https://aka.ms/arm-debug for usage details.",
            ///       "details": [
            ///         {
            ///           "code": "DeploymentOutputEvaluationFailed",
            ///           "target": "result",
            ///           "message": "The template output 'result' is not valid: Template output JToken type is not valid. Expected 'Integer'. Actual 'Null'.Please see https://aka.ms/arm-template/#outputs for usage details.."
            ///         }
            ///       ]
            ///     }
            ///   }
            /// }
            /// -- error --
            /// The template output 'result' is not valid:
            /// Template output JToken type is not valid.
            /// Expected 'Integer'.
            /// Actual 'Null'.
            /// Please see https://aka.ms/arm-template/#outputs for usage details..
            /// -- request --
            /// as above, but replace '"type": "Int"' => '"type": "Null"'
            /// -- response --
            /// HTTP/1.1 400 Bad Request
            /// {
            ///   "error": {
            ///     "code": "InvalidRequestContent",
            ///     "message": "The request content was invalid and could not be deserialized: 'Error converting value \"Null\" to type 'Azure.Deployments.Core.Entities.TemplateParameterType'. Path '', line 9, position 24.'."
            ///   }
            /// }
            /// </remarks>
            [Test]
            public static void EmptyArrayShouldThrow()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTestThrows(
                    "last(intersection(createArray('aaa'), createArray('bbb')))",
                    typeof(InvalidOperationException),
                    "Template output JToken type is not valid. Expected 'Object'. Actual 'Null'." +
                    "Please see https://aka.ms/arm-template/#outputs for usage details."
                );
            }

            [Test]
            public static void SingleItemIntgerArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "last(createArray(100))",
                    100
                );
            }

            [Test]
            public static void MultiItemIntegerArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "last(createArray(100, 200))",
                    200
                );
            }

            [Test]
            public static void SingleItemStringArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "last(createArray('aaa'))",
                    "aaa"
                );
            }

            [Test]
            public static void MultiItemStringArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "last(createArray('aaa', 'bbb'))",
                    "bbb"
                );
            }

            [Test]
            public static void MixedValueArrayShouldWork()
            {
                ArmExpressionEvaluatorTests.AssertEvaluatorTest(
                    "last(createArray('aaa', 100))",
                    100
                );
            }

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

    }

}
