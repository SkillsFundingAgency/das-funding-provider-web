using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication
{
    public class TestAuthenticationOptions : ITestAuthenticationOptions
    {
        public List<Claim> Claims { get; private set; }

        public TestAuthenticationOptions(List<Claim> claims)
        {
            Claims = claims;
        }
    }
}
