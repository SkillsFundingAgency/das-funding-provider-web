using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SFA.DAS.Funding.Provider.Web.Infrastructure.HealthCheck;

namespace SFA.DAS.Funding.Provider.Web;

public static class HstsStartup
{
    public static IApplicationBuilder UseDasHsts(this IApplicationBuilder app)
    {
        var hostingEnvironment = app.ApplicationServices.GetService<IWebHostEnvironment>();

        if (!hostingEnvironment.IsDevelopment())
        {
            app.UseHsts();
        }

        return app;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
        });

        app.UseHealthChecks("/ping", new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = (context, _) =>
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("");
            }
        });

        return app;
    }
}