namespace Kingsland.ArmLinter.Tests.Helpers
{

    public sealed record ArmCredentials
    {

        #region Constructors

        public ArmCredentials(
            string tenantId, string clientDomain, string clientId, string clientSecret, string subscriptionId
        )
        {
            this.TenantId = tenantId;
            this.ClientDomain = clientDomain;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.SubscriptionId = subscriptionId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Directory ID / Tenant ID for the service principal
        /// in Azure Active Directory.
        /// </summary>
        public string TenantId
        {
            get;
            private init;
        }

        /// <summary>
        /// The Client Domain for the service principal
        /// in Azure Active Directory.
        /// </summary>
        public string ClientDomain
        {
            get;
            private init;
        }

        /// <summary>
        /// The Application ID / Client ID for the service principal
        /// in Azure Active Directory.
        /// </summary>
        public string ClientId
        {
            get;
            private init;
        }

        /// <summary>
        /// The Client Secret for the service principal
        /// in Azure Active Directory.
        /// </summary>
        public string ClientSecret
        {
            get;
            private init;
        }

        /// <summary>
        /// </summary>
        public string SubscriptionId
        {
            get;
            private init;
        }

        #endregion

    }

}
