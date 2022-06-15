using SFA.DAS.Funding.Provider.Web.Services.Users.Types;
using SFA.DAS.HashingService;

namespace SFA.DAS.Funding.Provider.Web.Models
{
    public static class UserModelExtensions
    {
        public static IEnumerable<UserModel> ToUserModel(this IEnumerable<AccountUsers> dtos, IHashingService hashingService)
        {
            return dtos.Select(x => x.ToUserModel(hashingService));
        }

        public static UserModel ToUserModel(this AccountUsers dto, IHashingService hashingService)
        {
            return new UserModel
            {
                UserRef = dto.userRef,
                AccountId = hashingService.HashValue(dto.accountId)
            };
        }
    }
}