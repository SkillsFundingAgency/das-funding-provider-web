namespace SFA.DAS.Funding.Provider.Web.Services.Users.Types
{
    public class GetUserRequest
    {
        public Guid UserRef { get; set; }
        public IEnumerable<UserRole> Roles { get; set; }        
    }
}
