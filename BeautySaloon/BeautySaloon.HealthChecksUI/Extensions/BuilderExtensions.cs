using BeautySaloon.HealthChecksUI.HealthChecks;
using Serilog;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.Configure<HealthChecksSettings>(configuration.GetSection("HealthChecksSettings"));
        builder.Services.AddMemoryCache();
        // Add services to the container.
        builder.ConfigureSerilog();

        builder.Services.AddRazorPages();

        builder.ConfigureHealthChecks();

        builder.ConfigureHealthChecksUI();

        return builder;
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var rabbitUri = string.Empty;

        if (builder.Environment.IsDevelopment())
        {
            rabbitUri = "amqp://guest:guest@localhost:5672";
        }
        else
        {
            rabbitUri = "amqp://guest:guest@RabbitMQ";
        }
        builder.Services.AddHealthChecks()
                        .AddRabbitMQHealthCheck("API", new HealthChecksSettings
                        {
                            RequestQueueName = "request-health-api",
                            ReplyQueueName = "reply-health-api",
                            HostName = "beautysaloon-rabbit",
                            UserName = "guest",
                            Password = "guest",
                            Uri = rabbitUri
                        })
                        .AddRabbitMQHealthCheck("Identity", new HealthChecksSettings
                        {
                            RequestQueueName = "request-health-identity",
                            ReplyQueueName = "reply-health-identity",
                            HostName = "beautysaloon-rabbit",
                            UserName = "guest",
                            Password = "guest",
                            Uri = rabbitUri
                        })
                        .AddSqlServer(configuration.GetConnectionString("HealthChecksDb"),
                                      name: "Health Checks SQL Server",
                                      tags: new[] { "database" });
    }

    private static void ConfigureHealthChecksUI(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.SetMinimumSecondsBetweenFailureNotifications(5);
            options.UseApiEndpointHttpMessageHandler(sp =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                };
            });

            if (builder.Environment.IsDevelopment())
            {
                options.AddHealthCheckEndpoint("Healthcheck API", "/health");
            }
            else
            {
                options.AddHealthCheckEndpoint("Healthcheck API", "http://localhost/health");
            }

        }).AddSqlServerStorage(configuration.GetConnectionString("HealthChecksDb"), options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
               .ReadFrom.Configuration(context.Configuration)
               .Enrich.FromLogContext());
    }
}
