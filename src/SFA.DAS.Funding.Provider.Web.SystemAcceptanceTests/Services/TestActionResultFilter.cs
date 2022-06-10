using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services
{
    public class TestActionResultFilter : IAsyncAlwaysRunResultFilter
    {
        private readonly IHook<IActionResult> _actionResultHook;

        public TestActionResultFilter(IHook<IActionResult> actionResultHook)
        {
            _actionResultHook = actionResultHook;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {
                _actionResultHook.OnReceived(context.Result);
                await next();
                _actionResultHook.OnProcessed(context.Result);
            }
            catch (Exception ex)
            {
                _actionResultHook.OnErrored(ex, context.Result);
                throw;
            }
        }
    }
}
