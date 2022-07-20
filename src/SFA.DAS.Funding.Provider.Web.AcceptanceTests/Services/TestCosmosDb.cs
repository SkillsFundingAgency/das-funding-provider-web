using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.CosmosDb;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;

namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services
{
    public class TestCosmosDb
    {
        public TestCosmosDb(
            IWebHostBuilder builder,
            ICosmosDbConfiguration cosmosDbConfigurationOptions
            )
        {
            builder
                .ConfigureServices(s =>
                {
                    s.Configure<CosmosDbConfigurationOptions>(o =>
                    {
                        o.Uri = cosmosDbConfigurationOptions.Uri;
                        o.AuthKey = cosmosDbConfigurationOptions.AuthKey;
                    });
                });
        }
    }
}
