//using SFA.DAS.Configuration.AzureTableStorage;
//using SFA.DAS.Funding.Provider.Web.Infrastructure;

//namespace SFA.DAS.Funding.Provider.Web
//{
//    // [ExcludeFromCodeCoverage]
//    public class Startup
//    {
//        private readonly IWebHostEnvironment _environment;
//        private readonly IConfiguration _configuration;

//        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
//        {
//            _environment = environment;
//            var configBuilder = new ConfigurationBuilder()
//                .AddConfiguration(configuration)
//                .SetBasePath(Directory.GetCurrentDirectory())
//#if DEBUG
//                .AddJsonFile("appsettings.json", true)
//                .AddJsonFile("appsettings.development.json", true)
//#endif
//                .AddEnvironmentVariables();

//            if (!configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
//            {
//                configBuilder.AddAzureTableStorage(options =>
//                {
//                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
//                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
//                    options.EnvironmentName = configuration["EnvironmentName"];
//                    options.PreFixConfigurationKeys = false;
//                }
//                    );

//                _configuration = configBuilder.Build();
//            }
//            else
//            {
//                _configuration = configuration;
//            }
//        }

//        public static void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllersWithViews();
//            services.AddNLog();
//        }

//        public static void AddHttpsRedirection(IServiceCollection services, string environment)
//        {
//            services.AddHttpsRedirection(options =>
//            {
//                options.HttpsPort = environment == "LOCAL" ? 5001 : 443;
//            });
//        }
//    }
//}