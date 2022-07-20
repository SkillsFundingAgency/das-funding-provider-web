using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;

namespace SFA.DAS.Funding.Provider.Web.Services.ReadStore
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly Lazy<IDocumentClient> _documentClient;

        public DocumentClientFactory(IOptions<CosmosDbConfigurationOptions> configuration)
        {
            var cosmosDboptions = configuration.Value;

            _documentClient = new Lazy<IDocumentClient>(() =>
            new DocumentClient(
                new Uri(cosmosDboptions.Uri),
                cosmosDboptions.AuthKey,
                new ConnectionPolicy
                {
                    RetryOptions =
                    {
                        MaxRetryAttemptsOnThrottledRequests = 2,
                        MaxRetryWaitTimeInSeconds = 2
                    }
                })
            );
        }

        public IDocumentClient CreateDocumentClient()
        {
            return _documentClient.Value;
        }
    }
}
