using SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services;
using SFA.DAS.HashingService;
using System.Security.Claims;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.FundingProviderApi;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests
{
    public class TestContext : IDisposable
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestWebsite Website { get; set; }
        public TestCosmosDb ReadStore { get; set; }        
        public HttpClient WebsiteClient { get; set; }
        public TestFundingProviderApi FundingProviderApi { get; set; }
        public IHashingService HashingService { get; set; }
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
                    new(EmployerClaimTypes.UserId, TestData.User.AccountOwnerUserId.ToString()),
                    new(EmployerClaimTypes.Account, TestData.User.AuthenticatedHashedId),
                    new(EmployerClaimTypes.EmailAddress, "test@test.com"),
                    new(EmployerClaimTypes.GivenName, "FirstName"),
                    new(EmployerClaimTypes.FamilyName, "Surname"),
                    new(EmployerClaimTypes.DisplayName, "FirstName and Surname"),
                    new(EmployerClaimTypes.FamilyName, "Surname")
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


