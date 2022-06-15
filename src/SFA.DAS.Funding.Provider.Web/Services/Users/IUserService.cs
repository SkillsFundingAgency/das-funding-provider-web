using System.Security.Claims;
using SFA.DAS.Funding.Provider.Web.Models;
using SFA.DAS.Funding.Provider.Web.Services.Users.Types;

namespace SFA.DAS.Funding.Provider.Web.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<Claim>> GetClaims(Guid userRef);
        Task<IEnumerable<UserModel>> Get(GetUserRequest request, CancellationToken cancellationToken = new CancellationToken());
    }
}
