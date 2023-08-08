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

        builder.Services.AddHealthChecks()
                        .AddRabbitMQHealthCheck("API", options =>
                        {
                            options.RequestQueueName = "request-health-api";
                            options.ReplyQueueName = "reply-health-api";
                            options.HostName = "beautysaloon-rabbit";
                            options.UserName = "guest";
                            options.Password = "guest";
                            if (builder.Environment.IsDevelopment())
                            {
                                options.Uri = "amqp://guest:guest@localhost:5672";
                            }
                            else
                            {
                                options.Uri = "amqp://guest:guest@RabbitMQ";
                            }
                        })
                        .AddRabbitMQHealthCheck("Identity", options =>
                        {
                            options.RequestQueueName = "request-health-identity";
                            options.ReplyQueueName = "reply-health-identity";
                            options.HostName = "beautysaloon-rabbit";
                            options.UserName = "guest";
                            options.Password = "guest";
                            if (builder.Environment.IsDevelopment())
                            {
                                options.Uri = "amqp://guest:guest@localhost:5672";
                            }
                            else
                            {
                                options.Uri = "amqp://guest:guest@RabbitMQ";
                            }
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
