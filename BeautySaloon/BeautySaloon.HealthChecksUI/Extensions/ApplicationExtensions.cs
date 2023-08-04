using HealthChecks.UI.Client;
using HealthChecks.UI.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

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
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecksUI(options =>
        {
            options.UIPath = "/healthchecks-ui";
        });
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
