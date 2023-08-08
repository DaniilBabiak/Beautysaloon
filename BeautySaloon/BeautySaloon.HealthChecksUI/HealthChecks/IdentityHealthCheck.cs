using BeautySaloon.HealthChecksUI.RabbitMQ;
using BeautySaloon.Shared;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BeautySaloon.HealthChecksUI.HealthChecks;

public class IdentityHealthCheck : IHealthCheck, IDisposable
{
    private readonly RabbitMQSettings _rabbitmqSettings;
    private readonly HealthChecksSettings _healthChecksSettings;
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public IdentityHealthCheck(IOptions<RabbitMQSettings> rabbitmqSettings, IOptions<HealthChecksSettings> healthChecksSettings)
    {
        _rabbitmqSettings = rabbitmqSettings.Value;
        _healthChecksSettings = healthChecksSettings.Value;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitmqSettings.HostName,
            UserName = _rabbitmqSettings.UserName,
            Password = _rabbitmqSettings.Password,
            Uri = new Uri(_rabbitmqSettings.Uri)
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = new HealthCheckResult(HealthStatus.Unhealthy, "No respond from Identity");
        _channel.QueueDeclare(queue: _healthChecksSettings.IdentityRequestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: _healthChecksSettings.IdentityReplyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var correlationId = Guid.NewGuid().ToString();

        var properties = _channel.CreateBasicProperties();
        properties.ReplyTo = _healthChecksSettings.IdentityReplyQueueName;
        properties.CorrelationId = correlationId;

        var body = Encoding.UTF8.GetBytes("");

        _channel.BasicPublish(exchange: "", routingKey: _healthChecksSettings.IdentityRequestQueueName, basicProperties: properties, body: body);

        _channel.BasicConsume(queue: _healthChecksSettings.IdentityReplyQueueName, autoAck: true, consumer: _consumer);

        _consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());

                var report = JsonConvert.DeserializeObject<HealthCheckRabbitMQMessageModel>(response);

                var status = report.Status;

                var entryDataDictionary = new Dictionary<string, object>();
                var description = new StringBuilder();
                var duration = TimeSpan.MinValue;
                foreach (var entry in report.Entries)
                {
                    entryDataDictionary.Add(entry.Key, entry.Value);
                    description.Append(entry.Key);
                    description.Append(": ");
                    description.Append(entry.Value);
                    description.AppendLine();
                }

                result = new HealthCheckResult(report.Status,
                                                   description: description.ToString(),
                                                   data: entryDataDictionary);
            }
        };


        await Task.Delay(5000, cancellationToken);
        Dispose();
        return result;
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
