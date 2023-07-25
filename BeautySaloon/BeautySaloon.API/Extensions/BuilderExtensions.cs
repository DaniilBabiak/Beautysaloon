using BeautySaloon.API.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeautySaloon.API.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        builder.ConfigureSqlContexts();

        return builder;
    }

    private static void ConfigureSqlContexts(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("BeautysaloonDbConnection");

        builder.Services.AddDbContext<BeautySaloonContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}
