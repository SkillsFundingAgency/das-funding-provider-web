using Microsoft.AspNetCore.Authorization;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication
{
    public class TestAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationHandler _handler;
        private readonly IHook<AuthorizationHandlerContext> _authContextHook;

        public TestAuthorizationHandler(
            IAuthorizationHandler handler,
            IHook<AuthorizationHandlerContext> authContextHook)
        {
            _handler = handler;
            _authContextHook = authContextHook;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            try
            {
                _authContextHook.OnReceived(context);
                await _handler.HandleAsync(context);
                _authContextHook.OnProcessed(context);
            }
            catch (Exception ex)
            {
                _authContextHook.OnErrored(ex, context);
                throw;
            }
        }
    }
}
