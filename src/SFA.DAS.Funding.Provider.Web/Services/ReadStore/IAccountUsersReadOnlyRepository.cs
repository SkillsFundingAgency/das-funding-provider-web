using SFA.DAS.CosmosDb;
using SFA.DAS.Funding.Provider.Web.Services.Users.Types;

namespace SFA.DAS.Funding.Provider.Web.Services.ReadStore
{
    public interface IAccountUsersReadOnlyRepository : IReadOnlyDocumentRepository<AccountUsers>
    {
    }
}
