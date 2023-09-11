using BeautySaloon.ImagesAPI.Exceptions;
using BeautySaloon.Shared;
using GlobalExceptionHandler.WebApi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace BeautySaloon.ImagesAPI.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.ConfigureExceptionHandler();

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

    private static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseGlobalExceptionHandler(options =>
        {
            options.ContentType = "application/json";
            options.ResponseBody(s => JsonConvert.SerializeObject(new
            {
                Message = "An error occurred whilst processing your request"
            }));

            options.Map<NotFoundException>().ToStatusCode(StatusCodes.Status404NotFound)
                   .WithBody((ex, context) => JsonConvert.SerializeObject(new
                   {
                       ex.Message
                   }));
        });
    }
}
