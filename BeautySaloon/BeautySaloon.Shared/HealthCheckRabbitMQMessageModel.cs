using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BeautySaloon.Shared;
public class HealthCheckRabbitMQMessageModel
{
    public HealthStatus Status { get; set; }
    public Dictionary<string, string> Entries { get; set; } = new Dictionary<string, string>();
    public TimeSpan Duration { get; set; }
}
