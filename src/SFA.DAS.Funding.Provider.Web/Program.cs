using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Funding.Provider.Web;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using SFA.DAS.Funding.Provider.Web.Infrastructure.DataProtection;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);
AddHttpsRedirection(builder.Services, builder.Configuration["EnvironmentName"]);
Configure(builder);

static void Configure(WebApplicationBuilder builder)
{
    var webApplication = builder.Build();

    var configBuilder = builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables();

    if (!builder.Configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
    {
        configBuilder.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = builder.Configuration["ConfigNames"].Split(",");
                options.StorageConnectionString = builder.Configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = builder.Configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            }
        );
    }

    configBuilder.Build();

    if (!webApplication.Environment.IsDevelopment())
    {
        webApplication.UseExceptionHandler("/error/500");
        webApplication.UseDasHsts();
        webApplication.UseHealthChecks();
        webApplication.UseAuthentication();
    }
    else
    {
        webApplication.UseDeveloperExceptionPage();
    }

    webApplication.UseStaticFiles();

    webApplication.Use(async (context, next) =>
    {
        if (context.Response.Headers.ContainsKey("X-Frame-Options"))
        {
            context.Response.Headers.Remove("X-Frame-Options");
        }

        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

        await next();

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            // Re-execute the request so the user gets the error page
            var originalPath = context.Request.Path.Value;
            context.Items["originalPath"] = originalPath;
            context.Request.Path = "/error/404";
            await next();
        }
    });

    webApplication.UseRouting();

    webApplication.UseAuthorization();

    webApplication.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    webApplication.Run();
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = _ => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    services.AddControllersWithViews();

    services.AddSingleton<IAuthorizationHandler, ProviderAuthorisationHandler>();

    services.AddAuthorization<DefaultAuthorizationContextProvider>();
    services.AddAuthorisationServicePolicies();

    if (UseAuthenticationStub(configuration))
    {
        services.AddProviderStubAuthentication();
    }
    else
    {
        if (!IsDevOrLocalEnvironment(configuration))
        {
            var providerConfig = configuration
                .GetSection(nameof(ProviderIdams))
                .Get<ProviderIdams>();
            services.AddAndConfigureProviderAuthentication(providerConfig);
        }
    }

    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

    services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

    services.AddMvc(
            options =>
            {
                options.Filters.Add(new AuthorizeFilter(PolicyNames.HasProviderAccount));
                options.EnableEndpointRouting = false;
                options.SuppressOutputFormatterBuffering = true;

                if (!IsDevOrLocalEnvironment(configuration))
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }
            })
        .AddControllersAsServices();


    services.AddHealthChecks();
    services.AddDataProtection(configuration);

    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
    });

    services.AddApplicationInsightsTelemetry(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
}

static bool IsDevOrLocalEnvironment(IConfiguration configuration)
{
    return configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
           || configuration["EnvironmentName"].Equals("DEVELOPMENT", StringComparison.CurrentCultureIgnoreCase);
}

static void AddHttpsRedirection(IServiceCollection services, string environment)
{
    services.AddHttpsRedirection(options => options.HttpsPort = environment == "LOCAL" ? 5001 : 443);
}

static bool UseAuthenticationStub(IConfiguration configuration)
{
    return configuration["StubProviderAuth"] != null && configuration["StubProviderAuth"].Equals("true", StringComparison.CurrentCultureIgnoreCase);
}

#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable S3903 // Types should be defined in named namespaces
public partial class Program { }
#pragma warning restore S3903 // Types should be defined in named namespaces
#pragma warning restore S1118 // Utility classes should not have public constructors

