using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.FundingProviderApi;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "fundingProviderApi")]
    public class FundingProviderApi
    {
        private readonly TestContext _context;

        public FundingProviderApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void InitialiseApi()
        {
            _context.FundingProviderApi = new TestFundingProviderApi();            
        }
    }
}
