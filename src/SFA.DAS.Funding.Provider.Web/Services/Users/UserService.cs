using System.Security.Claims;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.CosmosDb;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication;
using SFA.DAS.Funding.Provider.Web.Models;
using SFA.DAS.Funding.Provider.Web.Services.ReadStore;
using SFA.DAS.Funding.Provider.Web.Services.Users.Types;
using SFA.DAS.HashingService;

namespace SFA.DAS.Funding.Provider.Web.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IAccountUsersReadOnlyRepository _accountUsersRepository;
        private readonly IHashingService _hashingService;

        public UserService(
            IAccountUsersReadOnlyRepository accountUsersRepository,
            IHashingService hashingService)
        {
            _accountUsersRepository = accountUsersRepository;
            _hashingService = hashingService;
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid userRef)
        {
            var claims = new List<Claim>();

            var users = await Get(
                new GetUserRequest()
                {
                    UserRef = userRef,
                    Roles = new List<UserRole>() { UserRole.Owner, UserRole.Transactor }
                });

            if (!users.Any())
            {
                return claims;
            }

            users.ToList().ForEach(u => claims.Add(new Claim(EmployerClaimTypes.Account, u.AccountId)));

            return claims;
        }

        public async Task<IEnumerable<UserModel>> Get(GetUserRequest request, CancellationToken cancellationToken = default)
        {
            var options = new FeedOptions { EnableCrossPartitionQuery = true };
            var acccountUsers = await _accountUsersRepository
                    .CreateQuery(options)
                    .Where(r =>
                        r.userRef == request.UserRef &&
                        r.removed == null &&
                        r.role != null &&
                        request.Roles.Contains(r.role.Value))
                    .ToListAsync();

            return acccountUsers.ToUserModel(_hashingService);
        }
    }
}
