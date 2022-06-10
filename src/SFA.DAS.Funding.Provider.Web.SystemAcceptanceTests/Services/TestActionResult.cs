using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services
{
    public class TestActionResult
    {
        public IActionResult LastActionResult { get; private set; } = null!;
        public ViewResult LastViewResult { get; private set; } = null!;
        public Exception LastException { get; private set; } = null!;

        public void SetActionResult(IActionResult actionResult)
        {
            LastActionResult = actionResult;
            if (actionResult is ViewResult result)
            {
                LastViewResult = result;
            }
        }

        public void SetException(Exception exception)
        {
            LastException = exception;
        }
    }
}
