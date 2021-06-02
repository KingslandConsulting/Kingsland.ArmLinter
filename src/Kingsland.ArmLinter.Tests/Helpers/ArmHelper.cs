using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingsland.ArmLinter.Tests.Helpers
{

    public static class ArmHelper
    {

        /// <summary>
        /// Authenticate using the app credentials.
        /// </summary>
        public static ServiceClientCredentials CreateServiceClientCredentials(
            this ArmCredentials credentials
        )
        {
            var clientCredential = new ClientCredential(
                credentials.ClientId, credentials.ClientSecret
            );
            var serviceCredentials = ApplicationTokenProvider.LoginSilentAsync(
                credentials.ClientDomain, clientCredential
            ).GetAwaiter().GetResult();
            return serviceCredentials;
        }

        public static Dictionary<string, object> ExecuteDeployment(ArmCredentials credentials, string subscriptionId, string resourceGroupName, string deploymentName, string armTemplateJson)
        {

            var azureCredentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(
                    credentials.ClientId, credentials.ClientSecret, credentials.TenantId,
                    AzureEnvironment.AzureGlobalCloud
                );

            var azure = Azure.Configure()
                .Authenticate(azureCredentials)
                .WithSubscription(subscriptionId);

            var resourceGroup = azure.ResourceGroups
                .GetByName(resourceGroupName);

            var deployment = azure.Deployments
                .Define(deploymentName)
                .WithExistingResourceGroup(resourceGroup.Name)
                .WithTemplate(armTemplateJson)
                .WithParameters("{}")
                .WithMode(DeploymentMode.Incremental)
                .Create();

            // outputs: {
            //   "my-output": {
            //     "type": "Array",
            //     "value": [ 1, 2, 3 ]
            //   }
            // }
            var outputs = ((JObject)deployment.Outputs).Children()
                .Cast<JProperty>()
                .ToDictionary(
                    // my-output
                    property => property.Name,
                    // new object[] { 1, 2, 3 }
                    property => property.Value.Value<object>("value") switch {
                        JArray arr =>
                            (object)arr.Values()
                                .Select(v => v.ToObject<object>())
                                .ToArray(),
                        JValue val =>
                            val.ToObject<object>(),
                        _ =>
                            throw new NotImplementedException()
                    }
                );

            return outputs;

        }

    }

}
