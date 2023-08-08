using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Shared;
public class HealthCheckRabbitMQMessageModel
{
    public HealthStatus Status { get; set; }
    public Dictionary<string, string> Entries { get; set; } = new Dictionary<string, string>();
    public TimeSpan Duration { get; set; }
}
