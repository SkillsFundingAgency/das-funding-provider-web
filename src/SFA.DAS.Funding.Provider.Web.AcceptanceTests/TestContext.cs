using SFA.DAS.Encoding;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using SFA.DAS.Funding.Provider.Web.AcceptanceTests.Hooks;
using SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services;
using SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services.FundingProviderApi;
using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestWebsite Website { get; set; }
        public TestCosmosDb ReadStore { get; set; }        
        public HttpClient WebsiteClient { get; set; }
        public TestFundingProviderApi FundingProviderApi { get; set; }
        public IEncodingService HashingService { get; set; }
        public TestDataStore TestDataStore { get; set; }
        public List<IHook> Hooks { get; set; }
        public List<Claim> Claims { get; set; }
        public TestActionResult ActionResult { get; set; }
        public WebConfigurationOptions WebConfigurationOptions { get; set; }
        public ExternalLinksConfiguration ExternalLinksOptions { get; set; }
        public CosmosDbConfigurationOptions CosmosDbConfigurationOptions { get; set; }
        
        private bool _isDisposed;

        public void AddOrReplaceClaim(string type, string value)
        {
            var existing = Claims.SingleOrDefault(c => c.Type == type);
            if(existing != null)
            {
                Claims.Remove(existing);
            }
            Claims.Add(new Claim(type, value));
        }

        public TestContext()
        {
            TestDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()));
            if (!TestDirectory.Exists)
            {
                Directory.CreateDirectory(TestDirectory.FullName);
            }
            TestDataStore = new TestDataStore();
            Hooks = new List<IHook>();

            Claims = new List<Claim>
                {
                    new(ProviderClaims.UserId, TestData.User.AccountOwnerUserId.ToString()),
                    new(ProviderClaims.Service, "Service"),
                    new(ProviderClaims.DisplayName, "FirstName and Surname"),
                    new(ProviderClaims.UserId, "UserId")
                };

            WebConfigurationOptions = new WebConfigurationOptions();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                WebsiteClient?.Dispose();
                FundingProviderApi?.Dispose();
            }

            _isDisposed = true;
        }
    }
}


