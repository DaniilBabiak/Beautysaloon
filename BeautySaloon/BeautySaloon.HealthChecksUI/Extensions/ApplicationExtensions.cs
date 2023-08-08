using HealthChecks.UI.Client;
using HealthChecks.UI.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MigrateDatabase();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseStaticFiles();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes = new Dictionary<HealthStatus, int>
                                {
                                    {HealthStatus.Healthy, StatusCodes.Status200OK},
                                    {HealthStatus.Degraded, StatusCodes.Status200OK},
                                    {HealthStatus.Unhealthy, StatusCodes.Status200OK},
                                },
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = _ => true
        });

        app.MapHealthChecksUI(options =>
        {
            options.UIPath = "/healthchecks-ui";
            options.ApiPath = "/healthchecks-api";
        }).RequireAuthorization("AdminPolicy");
        app.MapControllers();
        app.MapRazorPages();

        return app;
    }

    private static void MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<HealthChecksDb>())
            {
                try
                {
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
