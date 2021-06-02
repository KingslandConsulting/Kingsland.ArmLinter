using System;
using System.Collections.Generic;

namespace Kingsland.ArmLinter.Models
{

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Base on https://docs.microsoft.com/en-us/dotnet/api/microsoft.rest.azure.cloudexception?view=azure-dotnet
    /// </remarks>
    public sealed class CloudException : Exception
    {

        public CloudException(CloudError body, string message)
            : base(message)
        {
            this.Body = body;
        }

        public CloudError Body
        {
            get;
            private init;
        }

        //public static CloudException InvalidTemplate(string message)
        //{
        //    return new CloudException(
        //        new CloudError(
        //            "InvalidTemplate",
        //            new List<CloudError>(),
        //            message
        //        ),
        //        message
        //    );
        //}

        //public static CloudException DeploymentOutputEvaluationFailed(string outputName, string message)
        //{
        //    return new CloudException(
        //        new CloudError(
        //            "DeploymentOutputEvaluationFailed",
        //            new List<CloudError>(),
        //            message
        //        ),
        //        message
        //    );
        //}

    }

}
