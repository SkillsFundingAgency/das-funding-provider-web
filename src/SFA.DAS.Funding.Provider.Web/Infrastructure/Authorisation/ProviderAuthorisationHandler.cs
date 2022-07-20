using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation
{
    [ExcludeFromCodeCoverage]
    public class ProviderAuthorisationHandler : AuthorizationHandler<ProviderUkPrnRequirement>
    {
        private const string UkprnRootValue = "ukprn";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderAuthorisationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProviderUkPrnRequirement requirement)
        {
            if (!IsProviderAuthorised(context))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private bool IsProviderAuthorised(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                return false;
            }

            if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(UkprnRootValue))
            {
                var ukPrnFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[UkprnRootValue].ToString();
                var ukPrn = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

                return ukPrn.Equals(ukPrnFromUrl);
            }

            return true;
        }
    }
}
