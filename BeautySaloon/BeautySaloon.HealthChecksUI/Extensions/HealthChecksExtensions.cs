using BeautySaloon.HealthChecksUI.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddRabbitMQHealthCheck(
        this IHealthChecksBuilder builder,
        string name,
        HealthChecksSettings settings = null,
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = null,
        TimeSpan? timeout = null)
    {
        return builder.Add(new HealthCheckRegistration(name,
            sp =>
            {
                return new RabbitMQHealthCheck(settings);
            },
            failureStatus,
            tags,
            timeout));
    }
}
