﻿using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using GlobalExceptionHandler.WebApi;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace BeautySaloon.API.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MigrateDatabase();

        app.UseSerilogRequestLogging();
        app.UseCors("AllowAllPolicy");
        // Configure the HTTP request pipeline.

        app.ConfigureExceptionHandler();

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

            options.Map<ReservationNotAvailableException>().ToStatusCode(StatusCodes.Status400BadRequest)
                   .WithBody((ex, context) => JsonConvert.SerializeObject(new
                   {
                       ex.Message
                   }));

            options.Map<NotFoundException>().ToStatusCode(StatusCodes.Status404NotFound)
                   .WithBody((ex, context) => JsonConvert.SerializeObject(new
                   {
                       ex.Message
                   }));

            options.Map<ValidationException>().ToStatusCode(StatusCodes.Status400BadRequest)
                   .WithBody((ex, context) => JsonConvert.SerializeObject(new
                   {
                       ex.Message
                   }));
        });
    }

    private static void MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<BeautySaloonContext>())
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
