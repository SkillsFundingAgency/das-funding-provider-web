using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Funding.Provider.Web.ViewModels;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Extensions
{
    public static class AssertionsExtensions
    {
        public static ViewModelAssertions Should(this IViewModel instance)
        {
            return new ViewModelAssertions(instance);
        }

        public static ViewResultAssertions Should(this ViewResult instance)
        {
            return new ViewResultAssertions(instance);
        }
        public static HttpResponseMessageAssertions Should(this HttpResponseMessage instance)
        {
            return new HttpResponseMessageAssertions(instance);
        }
    }
}
