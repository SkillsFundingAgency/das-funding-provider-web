using SFA.DAS.CosmosDb;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration
{
    public class CosmosDbConfigurationOptions : ICosmosDbConfiguration
    {
        public const string CosmosDbConfiguration = "ReadStore";
        public virtual string Uri { get; set; }
        public virtual string AuthKey { get; set; }
    }
}
