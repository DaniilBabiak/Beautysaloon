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

        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();

        builder.ConfigureCors();

        builder.ConfigureHealthChecks();

        return builder;
    }

    private static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
                                .AddJwtBearer(options =>
                                {
                                    if (builder.Environment.IsDevelopment())
                                    {
                                        options.Authority = "https://localhost:5201";
                                    }
                                    else
                                    {
                                        options.Authority = "http://identity";
                                    }
                                    options.TokenValidationParameters.ValidateAudience = false;
                                    options.RequireHttpsMetadata = false;
                                });
    }

    private static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("HealthPolicy", policy =>
            {
                policy.RequireClaim("scope", "health");
            });
            options.AddPolicy("api.read", policy =>
            {
                policy.RequireClaim("scope", "api.read");
            });
            options.AddPolicy("api.edit", policy =>
            {
                policy.RequireClaim("scope", "api.edit");
                policy.RequireRole("admin");
            });
        });
    }
    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("BeautysaloonDbConnection");

        builder.Services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "API SQL Server",
                            tags: new[] { "Database" });
    }
    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(setup =>
        {
            setup.AddPolicy("AllowAllPolicy", options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });
        });
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
