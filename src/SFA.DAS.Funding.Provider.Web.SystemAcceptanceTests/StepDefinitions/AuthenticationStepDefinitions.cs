using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Authentication")]
    public class AuthenticationStepDefinitions : StepsBase
    {
        private readonly TestContext _testContext;
        private readonly TestDataStore _testDataStore;
        private AuthorizationHandlerContext _authContext;

        public AuthenticationStepDefinitions(TestContext testContext) : base(testContext)
        {
            _testContext = testContext;
            _testDataStore = _testContext.TestDataStore;
            var hook = _testContext.Hooks.SingleOrDefault(h => h is Hook<AuthorizationHandlerContext>) as Hook<AuthorizationHandlerContext>;
            hook.OnProcessed = (c) => {
                if (_authContext == null)
                {
                    _authContext = c;
                }
            };
        }

        [Given(@"a user of the system has not logged on")]
        public void GivenAUserOfTheSystemHasNotLoggedOn()
        {
            _testContext.Claims.Clear();
        }

        [When(@"the user access the (.*) page")]
        public async Task WhenTheUserAccessesTheHomePage(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _testContext.WebsiteClient.SendAsync(request);

            _testContext.TestDataStore.GetOrCreate("Response", onCreate: () =>
            {
                return response;
            });
        }

        [Then(@"the user is asked to log on")]
        public void ThenTheUserisAskedToLogOn()
        {
            var response = _testDataStore.Get<HttpResponseMessage>("Response");

            var challengeResult = _testContext.ActionResult.LastActionResult as ChallengeResult;
            challengeResult.Should().NotBeNull();

            _authContext.Requirements.Count().Should().Be(2);
            _authContext.Requirements.SingleOrDefault(r => r is IsAuthenticatedRequirement).Should().NotBeNull();
            _authContext.Requirements.SingleOrDefault(r => r is EmployerAccountRequirement).Should().NotBeNull();
        }
    }
}

