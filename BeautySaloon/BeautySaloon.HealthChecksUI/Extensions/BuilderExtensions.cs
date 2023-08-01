using BeautySaloon.HealthChecksUI.Helpers;
using Serilog;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.Configure<HealthChecksAuthSettings>(builder.Configuration.GetSection("HealthChecksAuthSettings"));

        // Add services to the container.
        builder.ConfigureSerilog();

        builder.Services.AddRazorPages();

        builder.Services.AddHealthChecks();
        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(60);
            options.SetMinimumSecondsBetweenFailureNotifications(60);
            options.UseApiEndpointHttpMessageHandler(sp =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                };
            });

            options.AddHealthCheckEndpoint("Identity", configuration["HealthCheckUris:Identity"]);
            options.AddHealthCheckEndpoint("Api", configuration["HealthCheckUris:API"]);
        }).AddSqlServerStorage(configuration.GetConnectionString("HealthChecksDb"), options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddTransient<HealthChecksHttpClientHandler>()
                        .AddHttpClient("health-checks") // HealthChecks.UI.Keys.HEALTH_CHECK_HTTP_CLIENT_NAME
                        .AddHttpMessageHandler<HealthChecksHttpClientHandler>();

        return builder;
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
               .ReadFrom.Configuration(context.Configuration)
               .Enrich.FromLogContext());
    }
}
