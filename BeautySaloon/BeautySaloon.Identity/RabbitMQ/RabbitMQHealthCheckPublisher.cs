using BeautySaloon.Shared;
using HealthChecks.UI.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace BeautySaloon.Identity.RabbitMQ;

public class RabbitMQHealthCheckPublisher : IHealthCheckPublisher
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMQSettings _options;

    public RabbitMQHealthCheckPublisher(IOptions<RabbitMQSettings> options)
    {
        _options = options.Value;
        _connectionFactory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Uri = new Uri(_options.Uri)
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: RabbitMQQueuesConfig.IdentityHealthQueue,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        var uiReport = UIHealthReport.CreateFrom(report);

        var message = JsonConvert.SerializeObject(uiReport);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: string.Empty,
                              routingKey: RabbitMQQueuesConfig.IdentityHealthQueue,
                              basicProperties: null,
                              body: body);

        return Task.CompletedTask;
    }
}
