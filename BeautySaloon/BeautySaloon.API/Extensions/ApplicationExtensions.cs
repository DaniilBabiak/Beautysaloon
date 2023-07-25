using BeautySaloon.API.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeautySaloon.API.Extensions;

public static class ApplicationExtensions
{
    public static async Task<WebApplication> ConfigureApplication(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapControllers();

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
