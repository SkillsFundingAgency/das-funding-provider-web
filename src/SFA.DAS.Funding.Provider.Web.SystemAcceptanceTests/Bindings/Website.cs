using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Hooks;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication;
using SFA.DAS.HashingService;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Bindings
{
    [Binding]
    public class Website
    {
        private readonly TestContext _context;

        public Website(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public void InitialiseWebsite()
        {
            var hook = new Hook<IActionResult>();
            var authHook = new Hook<AuthorizationHandlerContext>();
            _context.Hooks.Add(hook);
            _context.Hooks.Add(authHook);

            _context.WebConfigurationOptions.AllowedHashstringCharacters = "46789BCDFGHJKLMNPRSTVWXY";
            _context.WebConfigurationOptions.Hashstring = "Test hash-string";
            _context.WebConfigurationOptions.RedisCacheConnectionString = "localhost";
            _context.WebConfigurationOptions.DataEncryptionServiceKey = "P5T1NjQ1xqo1FgFM8RG+Yg==";
 
            _context.ExternalLinksOptions = new ExternalLinksConfiguration
            {
                CommitmentsSiteUrl = $"http://{Guid.NewGuid()}",
                ManageApprenticeshipSiteUrl = $"http://{Guid.NewGuid()}",
                EmployerRecruitmentSiteUrl = $"http://{Guid.NewGuid()}"
            };

            _context.Website = new TestWebsite(_context, hook, authHook);
            _context.WebsiteClient = _context.Website.CreateClient();
            _context.HashingService = (_context.Website.Services.GetService(typeof(IHashingService)) as IHashingService)!;

            var authmiddleware = _context.Website.Services.GetService(typeof(TestAuthenticationMiddleware));
        }
    }
}
