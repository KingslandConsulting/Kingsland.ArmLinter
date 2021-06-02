using Kingsland.ArmLinter.Models;
using Kingsland.ArmLinter.Tests.Helpers;
using Microsoft.Rest.Azure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Kingsland.ArmLinter.Tests
{

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public static partial class ArmExpressionEvaluatorTests
    {

        private static ArmCredentials GetClientCredentials()
        {
            var launchSettings = JsonDocument.Parse(
                File.ReadAllText(
                    ".\\Properties\\launchSettings.json"
                )
            );
            var environment = launchSettings
                .RootElement
                .GetProperty("profiles")
                .GetProperty("Kingsland.ArmLinter.Tests")
                .GetProperty("environmentVariables");
            var credentials = new ArmCredentials(
                tenantId: environment.GetProperty("TENANT_ID").GetString(),
                clientDomain: environment.GetProperty("CLIENT_DOMAIN").GetString(),
                clientId: environment.GetProperty("CLIENT_ID").GetString(),
                clientSecret: environment.GetProperty("CLIENT_SECRET").GetString(),
                subscriptionId: environment.GetProperty("SUBSCRIPTION_ID").GetString()
            );
            return credentials;
        }

        private static void AssertEvaluatorTest(string expression, object expected)
        {

            Assert.Multiple(
                () => {

                    // we have to evaluate the expression first so we know what type to expect
                    var evaluateResult = ArmExpressionEvaluator.Evaluate(expression);
                    Assert.AreEqual(expected, evaluateResult, "evaluator");

                    var credentials = ArmExpressionEvaluatorTests.GetClientCredentials();

                    var deploymentResult = ArmHelper.ExecuteDeployment(
                        credentials: credentials,
                        subscriptionId: "d160aa00-8080-4d57-aff9-839ad231bbb4",
                        resourceGroupName: "my-resourcegroup",
                        deploymentName: $"my-deployment-{RandomHelper.Next(0, 1000)}",
                        armTemplateJson: JsonSerializer.Serialize(
                            new Dictionary<string, object>
                            {
                                ["$schema"] = "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
                                ["contentVersion"] = "1.0.0.0",
                                ["resources"] = new List<object>(),
                                ["outputs"] = new Dictionary<string, object> {
                                    ["result"] = new Dictionary<string, object> {
                                        ["type"] = evaluateResult switch {
                                            //null => ArmTemplateParameterType.Object,
                                            Array _ => ArmTemplateParameterType.Array,
                                            bool => ArmTemplateParameterType.Bool,
                                            int => ArmTemplateParameterType.Int,
                                            long => ArmTemplateParameterType.Int,
                                            string => ArmTemplateParameterType.String,
                                            _ => throw new NotImplementedException()
                                        },
                                        ["value"] = $"[{expression}]"
                                    }
                                }
                            },
                            new JsonSerializerOptions
                            {
                                WriteIndented = true
                            }
                        )
                    );
                    Assert.AreEqual(expected, deploymentResult["result"], "deployment_result");

                }
            );

        }

        private static void AssertEvaluatorTestThrows(string expression, Models.CloudException expectedException)
        {

            Assert.Multiple(
                () =>
                {

                    var ex1 = Assert.Throws<Models.CloudException>(
                        () => {
                            var actual = ArmExpressionEvaluator.Evaluate(expression);
                        },
                        "evaluation_exception"
                    );
                    Assert.IsNotNull(ex1, "evaluation_null");
                    if (ex1 != null)
                    {
                        //Assert.AreEqual(
                        //    expectedException.Message,
                        //    ex1.Message,
                        //    "evaluation_message"
                        //);
                        Assert.AreEqual(
                            expectedException.Body.Code,
                            ex1.Body.Code,
                            "evaluation_body_code"
                        );
                        Assert.AreEqual(
                            expectedException.Body.Message,
                            ex1.Body.Message,
                            "evaluation_body_message"
                        );
                        Assert.AreEqual(
                            expectedException.Body.Details.Count,
                            ex1.Body.Details.Count,
                            "evaluation_details_count"
                        );
                        for (var i = 0; i < Math.Min(expectedException.Body.Details.Count, ex1.Body.Details.Count); i--)
                        {
                            Assert.AreEqual(
                                expectedException.Body.Details[i].Code,
                                ex1.Body.Details[i].Code,
                                $"evaluation_details[{i}]_code"
                            );
                            Assert.AreEqual(
                                expectedException.Body.Details[i].Message,
                                ex1.Body.Details[i].Message,
                                $"evaluation_details[{i}]_message"
                            );
                        }
                    }

                    var ex2 = Assert.Throws<Microsoft.Rest.Azure.CloudException>(
                        () => {
                            var credentials = ArmExpressionEvaluatorTests.GetClientCredentials();
                            var deploymentResult = ArmHelper.ExecuteDeployment(
                                credentials: credentials,
                                subscriptionId: "d160aa00-8080-4d57-aff9-839ad231bbb4",
                                resourceGroupName: "my-resourcegroup",
                                deploymentName: $"my-deployment-{RandomHelper.Next(0, 100000)}",
                                armTemplateJson: JsonSerializer.Serialize(
                                    new Dictionary<string, object>
                                    {
                                        ["$schema"] = "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
                                        ["contentVersion"] = "1.0.0.0",
                                        ["resources"] = new List<object>(),
                                        ["outputs"] = new Dictionary<string, object>
                                        {
                                            ["result"] = new Dictionary<string, object>
                                            {
                                                ["type"] = "Object",
                                                ["value"] = $"[{expression}]"
                                            }
                                        }
                                    },
                                    new JsonSerializerOptions
                                    {
                                        WriteIndented = true
                                    }
                                )
                            );
                        },
                        "evaluation_exception"
                    );
                    Assert.IsNotNull(ex2, "evaluation_null");

                    if (ex2 != null)
                    {
                        //Assert.AreEqual(
                        //    expectedException.Message,
                        //    ex2.Message,
                        //    "deployment_message"
                        //);
                        Assert.AreEqual(
                            expectedException.Body.Code,
                            ex2.Body.Code,
                            "deployment_body_code"
                        );
                        Assert.AreEqual(
                            expectedException.Body.Message,
                            ex2.Body.Message,
                            "deployment_body_message"
                        );
                        Assert.AreEqual(
                            expectedException.Body.Details.Count,
                            ex2.Body.Details.Count,
                            "deployment_body_details_count"
                        );
                        for (var i = 0; i < Math.Min(expectedException.Body.Details.Count, ex2.Body.Details.Count); i--)
                        {
                            Assert.AreEqual(
                                expectedException.Body.Details[i].Code,
                                ex2.Body.Details[i].Code,
                                $"deployment_details[{i}]_code"
                            );
                            Assert.AreEqual(
                                expectedException.Body.Details[i].Message,
                                ex2.Body.Details[i].Message,
                                $"deployment_details[{i}]_message"
                            );
                        }
                    }

                }
            );

        }

        private static void AssertEvaluatorTestThrows(string expression, Type expectedType, string expectedMessage)
        {

            Assert.Multiple(
                () =>
                {

                    var ex1 = Assert.Throws(
                        expectedType,
                        () => {
                            var actual = ArmExpressionEvaluator.Evaluate(expression);
                        },
                        "evaluation_exception"
                    );
                    Assert.IsNotNull(ex1, "evaluation_null");
                    if (ex1 != null)
                    {
                        Assert.AreEqual(expectedMessage, ex1.Message, "evaluation_message");
                    }

                    var deploymentResult = default(Dictionary<string, object>);
                    var ex2 = Assert.Throws<Microsoft.Rest.Azure.CloudException>(
                        () => {
                            var credentials = ArmExpressionEvaluatorTests.GetClientCredentials();
                            deploymentResult = ArmHelper.ExecuteDeployment(
                                credentials: credentials,
                                subscriptionId: credentials.SubscriptionId,
                                resourceGroupName: "my-resourcegroup",
                                deploymentName: $"my-deployment-{RandomHelper.Next(0, 100000)}",
                                armTemplateJson: JsonSerializer.Serialize(
                                    new Dictionary<string, object>
                                    {
                                        ["$schema"] = "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
                                        ["contentVersion"] = "1.0.0.0",
                                        ["resources"] = new List<object>(),
                                        ["outputs"] = new Dictionary<string, object>
                                        {
                                            ["result"] = new Dictionary<string, object>
                                            {
                                                ["type"] = "Object",
                                                ["value"] = $"[{expression}]"
                                            }
                                        }
                                    },
                                    new JsonSerializerOptions
                                    {
                                        WriteIndented = true
                                    }
                                )
                            );
                        },
                        "evaluation_exception"
                    );
                    Assert.IsNotNull(ex2, "evaluation_null");

                    if (ex2 != null)
                    {
                        Assert.AreEqual(1, ex2.Body.Details.Count, "deployment_message_count");
                        if (ex2.Body.Details.Count > 0)
                        {
                            var deploymentMessage = $"The template output 'result' is not valid: {expectedMessage}.";
                            Assert.AreEqual(deploymentMessage, ex2.Body.Details[0].Message, "deployment_message");
                        }
                    }

                }
            );

        }

    }

}
