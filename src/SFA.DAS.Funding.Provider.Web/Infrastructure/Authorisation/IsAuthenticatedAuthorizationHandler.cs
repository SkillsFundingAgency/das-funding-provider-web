using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation
{
    public class IsAuthenticatedAuthorizationHandler : AuthorizationHandler<IsAuthenticatedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthenticatedRequirement requirement)
        {
            var user = context.User;
            var userIsAnonymous = user?.Identity == null || !user.Identities.Any(i => i.IsAuthenticated);
            if (!userIsAnonymous)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
