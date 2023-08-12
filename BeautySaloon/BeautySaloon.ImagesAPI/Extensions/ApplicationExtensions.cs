using BeautySaloon.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BeautySaloon.ImagesAPI.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuthorization();
        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).RequireAuthorization("health");
        app.MapControllers().RequireAuthorization(ScopesConfig.ImageRead.Name);

        return app;
    }
}
