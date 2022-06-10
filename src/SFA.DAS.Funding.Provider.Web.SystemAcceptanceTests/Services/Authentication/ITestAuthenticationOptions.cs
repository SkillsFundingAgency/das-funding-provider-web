using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication
{
    public interface ITestAuthenticationOptions
    {
        List<Claim> Claims { get; }
    }
}
