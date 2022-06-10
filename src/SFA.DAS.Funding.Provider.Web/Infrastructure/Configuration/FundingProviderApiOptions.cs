using SFA.DAS.Http.Configuration;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration
{
    public class FundingProviderApiOptions : IApimClientConfiguration
    {
        public const string FundingProviderApi = "FundingProviderApi";

        public string ApiBaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string ApiVersion { get; set; }
    }
}
