using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.Funding.Provider.Web.Services.Users.Types;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests;

namespace SFA.DAS.Funding.Provider.Web.MockServer.CosmosDb
{
    public class AccountsReadStore
    {
        public DocumentClient Client { get; }
        public AccountsReadStore(DocumentClient client)
        {
            Client = client;
        }
    }

    public class AccountsReadStoreBuilder
    {
        private DocumentClient _documentClient;
        private Uri _databaseLink;

        public static Task<AccountsReadStoreBuilder> Create(int port = 8081)
        {
            var builder = new AccountsReadStoreBuilder();
            return builder.Initialise(port);
        }

        private async Task<AccountsReadStoreBuilder> Initialise(int port)
        {
            var authKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var dbId = "SFA.DAS.FundingProvider.Web.MockServer";

            _documentClient = new DocumentClient(
                new Uri($"https://localhost:{port}/"),
                authKey,
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                });

            await _documentClient.CreateDatabaseIfNotExistsAsync(
                new Database
                {
                    Id = dbId
                });

            _databaseLink = UriFactory.CreateDatabaseUri(dbId);

            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(
                _databaseLink,
                new DocumentCollection { Id = "AccountUsers" });

            return this;
        }

        public async Task<AccountsReadStoreBuilder> WithAccountForAccountOwnerUserId(long accountId)
        {
            var account = new AccountUsers
            {
                Id = TestData.User.AccountDocumentId,
                ETag = "00000000-0000-0000-0000-000000000000",
                userRef = TestData.User.AccountOwnerUserId,
                accountId = accountId
            };

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri("SFA.DAS.FundingProvider.Web.MockServer", "AccountUsers");

            await _documentClient.UpsertDocumentAsync(documentCollectionUri, account);

            return this;
        }

        public AccountsReadStore Build()
        {
            return new AccountsReadStore(null!);
        }
    }
}
