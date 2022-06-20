using NLog.Extensions.Logging;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Logging;

public static class LoggingServiceCollectionExtensions
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