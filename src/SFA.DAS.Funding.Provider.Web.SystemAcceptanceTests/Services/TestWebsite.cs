using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services
{
    public class TestWebsite : WebApplicationFactory<Program>
    {
        private readonly TestContext _testContext;
        private readonly Dictionary<string, string> _appConfig;
        private readonly IHook<IActionResult> _actionResultHook;
        private readonly IHook<AuthorizationHandlerContext> _authContextHook;
        public IWebHostBuilder? WebHostBuilder { get; private set; }

        public TestWebsite(
            TestContext testContext,
            IHook<IActionResult> actionResultHook,
            IHook<AuthorizationHandlerContext> authContextHook)
        {
            _testContext = testContext;
            _actionResultHook = actionResultHook;
            _authContextHook = authContextHook;

            _appConfig = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL" },
                { "Identity:ClientId", "dev" },
                { "Identity:ClientSecret", "secret" },
                { "Identity:BaseAddress", @"https://localhost:8082/identity" },
                { "Identity:Scopes", "openid profile" },
                { "Identity:UsePkce", "false" },
                { "ProviderIdams:MetadataAddress", @"https://localhost:8082/identity" },
                { "ProviderIdams:Wtrealm", "AppID" },
            };
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            WebHostBuilder = builder;

            builder
                .ConfigureAppConfiguration(a =>
                {
                    a.Sources.Clear();
                    a.AddInMemoryCollection(_appConfig);
                });

            builder
                .ConfigureServices(s =>
                {
                    // s.AddTransient<TestAuthenticationMiddleware>();
                    s.AddScoped<ITestAuthenticationOptions, TestAuthenticationOptions>(_ => new TestAuthenticationOptions(_testContext.Claims));
                    s.AddTransient<IStartupFilter, TestAuthenticationMiddlewareStartupFilter>();

                    s.Configure<WebConfigurationOptions>(o =>
                    {
                        o.AllowedHashstringCharacters = _testContext.WebConfigurationOptions.AllowedHashstringCharacters;
                        o.Hashstring = _testContext.WebConfigurationOptions.Hashstring;
                        o.DataEncryptionServiceKey = _testContext.WebConfigurationOptions.DataEncryptionServiceKey;
                    });
                    s.Configure<ExternalLinksConfiguration>(o =>
                    {

                        o.ManageApprenticeshipSiteUrl = _testContext.ExternalLinksOptions.ManageApprenticeshipSiteUrl;
                        o.CommitmentsSiteUrl = _testContext.ExternalLinksOptions.CommitmentsSiteUrl;
                        o.EmployerRecruitmentSiteUrl = _testContext.ExternalLinksOptions.EmployerRecruitmentSiteUrl;
                    });
                    s.Configure<FundingProviderApiOptions>(o =>
                      {
                          o.ApiBaseUrl = _testContext.FundingProviderApi.BaseAddress;
                          o.SubscriptionKey = "";
                      });
                    s.Configure<CosmosDbConfigurationOptions>(o =>
                    {
                        o.Uri = "https://localhost:8081/";
                        o.AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                    });
                    s.AddControllersWithViews(options =>
                    {
                        options.Filters.Add(new TestActionResultFilter(_actionResultHook));
                    });

                    s.Decorate<IAuthorizationHandler>((handler, _) => new TestAuthorizationHandler(handler, _authContextHook));

                });
        }
    }
}
