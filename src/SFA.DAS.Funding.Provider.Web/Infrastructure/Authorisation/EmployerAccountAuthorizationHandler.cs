using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication;
using SFA.DAS.Funding.Provider.Web.RouteValues;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation
{
    public class EmployerAccountAuthorizationHandler : AuthorizationHandler<EmployerAccountRequirement>
    {       

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
        {
            var isAuthorised = await IsEmployerAuthorised(context);
            if (isAuthorised)
            {
                context.Succeed(requirement);
            }            
        }

        private Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context)
        {
            if (!(context.Resource is AuthorizationFilterContext mvcContext))
            {
                return Task.FromResult(false);
            }

            if (mvcContext.RouteData.Values["controller"].Equals("Home") &&
                mvcContext.RouteData.Values["action"].Equals("Login")
                )
            {
                return Task.FromResult(true);
            }

            if (!mvcContext.RouteData.Values.ContainsKey(RouteValueKeys.AccountHashedId))
            {
                return Task.FromResult(false);
            }

            var accountIdFromUrl = mvcContext.RouteData.Values[RouteValueKeys.AccountHashedId].ToString().ToUpper();
            var userIdClaim = context.User.FindFirst(c => c.Type.Equals(EmployerClaimTypes.UserId));
            if (userIdClaim?.Value == null)
            {
                return Task.FromResult(false);
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Task.FromResult(false);
            }

            var accountClaim = context.User.FindFirst(c =>
                c.Type.Equals(EmployerClaimTypes.Account) &&
                c.Value.Equals(accountIdFromUrl, StringComparison.InvariantCultureIgnoreCase)
                );

            if (accountClaim?.Value != null)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
