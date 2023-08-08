using BeautySaloon.HealthChecksUI.HealthChecks;
using Microsoft.Extensions.Options;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddRabbitMQHealthCheck(
        this IHealthChecksBuilder builder,
        string name,
        Action<HealthChecksSettings> configureOptions = null)
    {
        builder.Services.AddTransient(provider =>
        {
            var settingsOptions = provider.GetRequiredService<IOptions<HealthChecksSettings>>();
            return ActivatorUtilities.CreateInstance<RabbitMQHealthCheck>(provider, configureOptions);
        });

        return builder.AddCheck<RabbitMQHealthCheck>(name);
    }
}
