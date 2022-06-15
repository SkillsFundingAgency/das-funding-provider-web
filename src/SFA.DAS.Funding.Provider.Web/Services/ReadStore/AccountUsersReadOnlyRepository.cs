using Microsoft.Azure.Documents;
using SFA.DAS.CosmosDb;
using SFA.DAS.Funding.Provider.Web.Services.ReadStore.Types;
using SFA.DAS.Funding.Provider.Web.Services.Users.Types;

namespace SFA.DAS.Funding.Provider.Web.Services.ReadStore
{
    public class AccountUsersReadOnlyRepository : DocumentRepository<AccountUsers>, IAccountUsersReadOnlyRepository
    {
        public AccountUsersReadOnlyRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountUsersCollectionName)
        {
        }

        public AccountUsersReadOnlyRepository(IDocumentClientFactory documentClientFactory)
            : base(documentClientFactory.CreateDocumentClient(), DocumentSettings.DatabaseName, DocumentSettings.AccountUsersCollectionName)
        {
        }
    }
}
