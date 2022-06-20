using Microsoft.AspNetCore.Http;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication
{
    public class TestAuthenticationMiddlewareForAchieveService
    {
        private readonly RequestDelegate _next;

        public TestAuthenticationMiddlewareForAchieveService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITestAuthenticationOptions options)
        {
            if (context.Request.Host.ToString() != "test.achieveservice.com" || options.Claims?.Count == 0)
            {
                await _next(context);
                return;
            }

            var accountClaim = options.Claims.SingleOrDefault(c => c.Type == ProviderClaims.UserId);

            options.Claims.Remove(accountClaim);
            options.Claims.Add(new Claim(ProviderClaims.UserId, "SERVICE"));
            
            await _next(context);

            options.Claims.Remove(options.Claims.SingleOrDefault(c => c.Type == ProviderClaims.UserId));
            options.Claims.Add(accountClaim);
        }
    }
}
