//using SFA.DAS.Configuration.AzureTableStorage;
//using SFA.DAS.Funding.Provider.Web;
//using SFA.DAS.Funding.Provider.Web.Infrastructure;

//var builder = WebApplication.CreateBuilder(args);

//Startup.ConfigureServices(builder.Services);
//Startup.AddHttpsRedirection(builder.Services, builder.Configuration["EnvironmentName"]);
//Startup.Configure(builder);

//public class Startup
//{
//    public static void Configure(WebApplicationBuilder builder)
//    {
//        var webApplication = builder.Build();

//        var configBuilder = builder.Configuration
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddEnvironmentVariables();

//        if (!builder.Configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
//        {
//            configBuilder.AddAzureTableStorage(options =>
//                {
//                    options.ConfigurationKeys = builder.Configuration["ConfigNames"].Split(",");
//                    options.StorageConnectionString = builder.Configuration["ConfigurationStorageConnectionString"];
//                    options.EnvironmentName = builder.Configuration["EnvironmentName"];
//                    options.PreFixConfigurationKeys = false;
//                }
//            );
//        }

//        configBuilder.Build();

//        if (!webApplication.Environment.IsDevelopment())
//        {
//            webApplication.UseExceptionHandler("/error/500");
//            webApplication.UseDasHsts();
//            webApplication.UseHealthChecks();
//            webApplication.UseAuthentication();
//        }
//        else
//        {
//            webApplication.UseDeveloperExceptionPage();
//        }

//        webApplication.UseStaticFiles();

//        webApplication.Use(async (context, next) =>
//        {
//            if (context.Response.Headers.ContainsKey("X-Frame-Options"))
//            {
//                context.Response.Headers.Remove("X-Frame-Options");
//            }

//            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

//            await next();

//            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
//            {
//                // Re-execute the request so the user gets the error page
//                var originalPath = context.Request.Path.Value;
//                context.Items["originalPath"] = originalPath;
//                context.Request.Path = "/error/404";
//                await next();
//            }
//        });

//        webApplication.UseRouting();

//        webApplication.UseAuthorization();

//        webApplication.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");

//        webApplication.Run();
//    }

//    public static void ConfigureServices(IServiceCollection services)
//    {
//        services.AddControllersWithViews();
//        services.AddNLog();
//    }

//    public static void AddHttpsRedirection(IServiceCollection services, string environment)
//    {
//        services.AddHttpsRedirection(options =>
//        {
//            options.HttpsPort = environment == "LOCAL" ? 5001 : 443;
//        });
//    }
//}

