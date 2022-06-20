using Microsoft.AspNetCore.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication
{
    [ExcludeFromCodeCoverage]
    public static class ProviderStubAuthentication
    {
        public static void AddProviderStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
                "Provider-stub",
                _ => { });
        }
    }
}
