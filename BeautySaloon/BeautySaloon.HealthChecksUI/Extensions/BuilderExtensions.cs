using BeautySaloon.HealthChecksUI.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        builder.Services.Configure<HealthChecksAuthSettings>(builder.Configuration.GetSection("HealthChecksAuthSettings"));
        builder.Services.AddMemoryCache();
        // Add services to the container.
        builder.ConfigureSerilog();

        builder.Services.AddRazorPages();

        builder.Services.AddHealthChecks()
                        .AddRabbitMQ(configuration.GetSection("RabbitMQSettings")["Uri"],
                                     name: "RabbitMQ",
                                     tags: new[] { "Queue services" });
        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(30);
            options.SetMinimumSecondsBetweenFailureNotifications(30);
            
            options.UseApiEndpointHttpMessageHandler(sp =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                };
            });
            options.AddHealthCheckEndpoint("Health checker", "http://localhost:80/health");
            options.AddHealthCheckEndpoint("Identity", "http://localhost:80/api/Health/Identity");
            options.AddHealthCheckEndpoint("Api", "http://localhost:80/api/Health/API");
        }).AddSqlServerStorage(configuration.GetConnectionString("HealthChecksDb"), options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddHostedService<HealthCheckMessageListener>();

        return builder;
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
               .ReadFrom.Configuration(context.Configuration)
               .Enrich.FromLogContext());
    }
}
