using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using GlobalExceptionHandler.WebApi;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace BeautySaloon.API.Extensions;

public static class ApplicationExtensions
{
    public static async Task<WebApplication> ConfigureApplication(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseCors("AllowAllPolicy");
        // Configure the HTTP request pipeline.
        app.UseGlobalExceptionHandler(options =>
        {
            options.ContentType = "application/json";
            options.ResponseBody(s => JsonConvert.SerializeObject(new
            {
                Message = "An error occurred whilst processing your request"
            }));

            options.Map<ReservationNotAvailableException>().ToStatusCode(StatusCodes.Status400BadRequest)
                .WithBody((ex, context) => JsonConvert.SerializeObject(new
                {
                    Message = ex.Message
                }));
        });
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            c.OAuthClientId("swagger");
            c.OAuthAppName("Swagger UI");
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapControllers().RequireAuthorization("api.read");
        //app.MapHealthChecks("/api/health", new HealthCheckOptions
        //{
        //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //});
        await app.MigrateDatabase();

        return app;
    }

    private async static Task MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<BeautySaloonContext>())
            {
                try
                {
                    await appContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
