using SFA.DAS.CosmosDb;

namespace SFA.DAS.Funding.Provider.Web.Services.Users.Types
{
    public class AccountUsers : IDocument
    {
        public Guid Id { get; set; }
        public string ETag { get; set; }
        public Guid userRef { get; set; }
        public long accountId { get; set; }
        public DateTime? removed { get; set; }
        public UserRole? role { get; set; }
    }
}
