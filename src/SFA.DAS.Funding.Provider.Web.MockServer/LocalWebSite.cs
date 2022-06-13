using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services.Authentication;
using System.Security.Claims;

namespace SFA.DAS.Funding.Provider.Web.MockServer
{
    public class LocalWebSite : IDisposable
    {
        private readonly List<Claim> _claims;
        private WebApplication _host;

        public LocalWebSite(List<Claim> claims)
        {
            _claims = claims;
        }

        public LocalWebSite Run()
        {
            _host.Run();
            return this;
        }

        public LocalWebSite Build()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ApplicationName = typeof(Program).Assembly.FullName,
                ContentRootPath = Directory.GetCurrentDirectory(),
                EnvironmentName = "LOCAL"
            });

            Startup.ConfigureServices(builder.Services);
            Startup.AddHttpsRedirection(builder.Services, "LOCAL");

            builder
                .Services.AddTransient<TestAuthenticationMiddleware>()
                    .AddScoped<ITestAuthenticationOptions, TestAuthenticationOptions>(_ => new TestAuthenticationOptions(_claims))
                    .AddTransient<IStartupFilter, TestAuthenticationMiddlewareStartupFilter>()
            ;

            _host = builder.Build();
            
            return this;
        }

        public void Dispose()
        {
            _host.DisposeAsync().ConfigureAwait(false);
        }
    }
}
