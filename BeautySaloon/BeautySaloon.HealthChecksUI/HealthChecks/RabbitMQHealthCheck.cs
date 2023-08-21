using BeautySaloon.Shared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace BeautySaloon.HealthChecksUI.HealthChecks;

public class RabbitMQHealthCheck : IHealthCheck, IDisposable
{
    private readonly HealthChecksSettings _settings = new HealthChecksSettings();
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public RabbitMQHealthCheck(HealthChecksSettings settings = null)
    {
        _settings = settings ?? new HealthChecksSettings();

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            Uri = new Uri(_settings.Uri)
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = new HealthCheckResult(HealthStatus.Unhealthy, $"No respond from {context.Registration.Name}");
        try
        {
            _channel.QueueDeclare(queue: _settings.RequestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: _settings.ReplyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var correlationId = Guid.NewGuid().ToString();

            var properties = _channel.CreateBasicProperties();
            properties.ReplyTo = _settings.ReplyQueueName;
            properties.CorrelationId = correlationId;

            var body = Encoding.UTF8.GetBytes("");

            _channel.BasicPublish(exchange: "", routingKey: _settings.RequestQueueName, basicProperties: properties, body: body);

            _channel.BasicConsume(queue: _settings.ReplyQueueName, autoAck: true, consumer: _consumer);

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
        }
        catch (Exception ex)
        {
            Log.Error($"No respond from {context.Registration.Name}");
        }



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
