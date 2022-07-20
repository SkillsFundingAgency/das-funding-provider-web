using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services.Authentication
{
    public interface ITestAuthenticationOptions
    {
        List<Claim> Claims { get; }
    }
}
