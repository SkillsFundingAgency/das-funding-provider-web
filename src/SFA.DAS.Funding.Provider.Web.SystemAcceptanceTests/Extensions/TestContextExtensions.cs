using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Extensions
{
    public static class TestContextExtensions
    {
        public static Task<WaitForResult> WaitFor<T>(
                    this TestContext context,
                    Func<Task> func,
                    bool assertOnTimeout = true,
                    bool assertOnError = false,
                    int timeoutInMs = 15000)
        {
            return new TestHelper(context)
                .WaitFor<T>(func, assertOnTimeout: assertOnTimeout, assertOnError: assertOnError, timeoutInMs: timeoutInMs);
        }     
    }
}
