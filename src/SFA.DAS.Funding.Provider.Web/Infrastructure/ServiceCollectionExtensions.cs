using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NLog.Extensions.Logging;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Logging;
using SFA.DAS.Funding.Provider.Web.Services.Users;

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

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(
                PolicyNames.IsAuthenticated,
                policy =>
                {
                    policy.Requirements.Add(new IsAuthenticatedRequirement());
                });

            options.AddPolicy(
                PolicyNames.HasEmployerAccount,
                policy =>
                {
                    policy.Requirements.Add(new EmployerAccountRequirement());
                });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddEmployerAuthentication(this IServiceCollection serviceCollection, IdentityServerOptions identityServerOptions)
    {
        serviceCollection.AddSingleton<IAuthorizationHandler, IsAuthenticatedAuthorizationHandler>();
        serviceCollection.AddSingleton<IAuthorizationHandler, EmployerAccountAuthorizationHandler>();

        _ = serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.AccessDeniedPath = new PathString("/error/403");
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.Name = CookieNames.AuthCookie;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.UsePkce = identityServerOptions.UsePkce;

                options.ClientId = identityServerOptions.ClientId;
                options.ClientSecret = identityServerOptions.ClientSecret;
                options.Authority = identityServerOptions.BaseAddress;
                options.MetadataAddress = $"{identityServerOptions.BaseAddress}/.well-known/openid-configuration";
                options.ResponseType = OpenIdConnectResponseType.Code;

                var scopes = identityServerOptions.Scopes.Split(' ');
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }

                options.ClaimActions.MapUniqueJsonKey("sub", "id");

                    // TODO: add redirect code ?
                    // https://auth0.com/docs/quickstart/webapp/aspnet-core-3/01-login                    
                });

        serviceCollection
            .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
            .Configure<IUserService>((options, userService) =>
            {
                options.Events.OnTokenValidated = async (ctx) => await PopulateAccountsClaim(ctx, userService);
            });

        serviceCollection
            .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
            .Configure<ILoggerFactory>((options, loggerFactory) =>
            {
                options.Events.OnRemoteFailure = async (ctx) => await OnRemoteFailure(ctx, loggerFactory);
            });

        return serviceCollection;
    }

    private static async Task PopulateAccountsClaim(TokenValidatedContext ctx, IUserService userService)
    {
        var userIdString = ctx.Principal.Claims
            .First(c => c.Type.Equals(EmployerClaimTypes.UserId))
            .Value;

        if (Guid.TryParse(userIdString, out Guid userId))
        {
            var claims = await userService.GetClaims(userId);

            claims.ToList().ForEach(c => ctx.Principal.Identities.First().AddClaim(c));
        }
    }

    private static Task OnRemoteFailure(RemoteFailureContext ctx, ILoggerFactory loggerFactory)
    {
        try
        {
            if (ctx.Failure.Message.Contains("Correlation failed"))
            {
                var redirectUri = ctx.Properties.RedirectUri;
                var logger = loggerFactory.CreateLogger("SFA.DAS.EmployerIncentives.Authentication");

                logger.LogError(ctx.Failure, $"Correlation failed error when redirecting from {redirectUri}");
            }
        }
        catch
        {
            // ignore errors
        }

        return Task.CompletedTask;
    }
}