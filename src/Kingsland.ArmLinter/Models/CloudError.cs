using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kingsland.ArmLinter.Models
{

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Based on https://docs.microsoft.com/en-us/dotnet/api/microsoft.rest.azure.clouderror?view=azure-dotnet
    /// </remarks>
    public sealed record CloudError
    {

        public CloudError(string code, IEnumerable<CloudError> details, string message)
        {
            this.Code = code;
            this.Details = new ReadOnlyCollection<CloudError>(
                details.ToList()
            );
            this.Message = message;
        }

        public string Code
        {
            get;
            private init;
        }

        public ReadOnlyCollection<CloudError> Details
        {
            get;
            private init;
        }

        public string Message
        {
            get;
            private init;
        }

    }

}
