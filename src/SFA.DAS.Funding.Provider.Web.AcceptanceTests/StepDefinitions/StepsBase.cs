using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.Funding.Provider.Web.AcceptanceTests.Hooks;
using SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests.StepDefinitions
{
    public class StepsBase
    {
        protected readonly TestContext TestContext;

        public StepsBase(TestContext testContext)
        {
            TestContext = testContext;
            var hook = testContext.Hooks.SingleOrDefault(h => h is Hook<IActionResult>) as Hook<IActionResult>;

            testContext.ActionResult = new TestActionResult();
            hook!.OnProcessed = (message) => { testContext.ActionResult.SetActionResult(message); };
            hook.OnErrored = (ex, _) => { testContext.ActionResult.SetException(ex); };
        }
    }
}
