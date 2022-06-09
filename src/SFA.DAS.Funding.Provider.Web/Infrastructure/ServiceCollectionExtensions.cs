using NLog.Extensions.Logging;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Logging;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNLog(this IServiceCollection serviceCollection)
    {
        var nLogConfiguration = new NLogConfiguration();

        serviceCollection.AddLogging(options =>
        {
            options.AddFilter("SFA.DAS", LogLevel.Information);
            options.SetMinimumLevel(LogLevel.Trace);
            options.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });
            options.AddConsole();

            nLogConfiguration.ConfigureNLog();
        });

        return serviceCollection;
    }
}