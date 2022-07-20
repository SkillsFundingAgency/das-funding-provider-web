using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.DataProtection
{
    [ExcludeFromCodeCoverage]
    public static class AddDataProtectionExtension
    {
        public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(WebConfigurationOptions.FundingProviderWebConfiguration))
                .Get<WebConfigurationOptions>();

            if (config != null
                && !string.IsNullOrEmpty(config.DataProtectionKeysDatabase)
                && !string.IsNullOrEmpty(config.RedisCacheConnectionString))
            {
                var redisConnectionString = config.RedisCacheConnectionString;
                var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

                services.AddDataProtection()
                    .SetApplicationName("das-funding-provider-web")
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
        }
    }
}
