using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.RabbitMQ;
using BeautySaloon.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace BeautySaloon.API.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));

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
                                options.Authority = "https://localhost:5001";
                            }
                            else
                            {
                                options.Authority = "http://identity";
                            }

                            options.TokenValidationParameters.ValidIssuers = new[] { "https://localhost:5001" };
                            options.TokenValidationParameters.ValidateAudience = false;
                            options.RequireHttpsMetadata = false;
                        });
    }

    private static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("api.read", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ApiRead.Name);
            });
            options.AddPolicy("api.edit", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ApiEdit.Name);
                policy.RequireAuthenticatedUser();
                policy.RequireRole(RolesConfig.Admin);
            });
        });
    }
    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        string connectionString = configuration.GetConnectionString("BeautysaloonDbConnection");
        
        builder.Services.AddSingleton<IHealthCheckPublisher, RabbitMQHealthCheckPublisher>();

        builder.Services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "API SQL Server",
                            tags: new[] { "Database" })
                        .AddRabbitMQ(configuration.GetSection("RabbitMQSettings")["Uri"],
                                     name: "RabbitMQ");
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
