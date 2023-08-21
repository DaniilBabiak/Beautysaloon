using BeautySaloon.Identity.RabbitMQ;
using BeautySaloon.Shared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace BeautySaloon.Identity.HealthChecks;

public class RabbitMQHealthCheckListener : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly HealthChecksSettings _healthChecksSettings;
    private readonly HealthCheckService _healthCheckService;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private EventingBasicConsumer _consumer;

    public RabbitMQHealthCheckListener(IOptions<RabbitMQSettings> settings, IOptions<HealthChecksSettings> healthChecksSettings, HealthCheckService healthCheckService)
    {
        _settings = settings.Value;
        _healthChecksSettings = healthChecksSettings.Value;
        _healthCheckService = healthCheckService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
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
                await ConnectAndExecute(stoppingToken);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while connecting to RabbitMQ.");

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }        
    }

    private async Task ConnectAndExecute(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(queue: _healthChecksSettings.IdentityRequestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.BasicConsume(queue: _healthChecksSettings.IdentityRequestQueueName, autoAck: true, consumer: _consumer);

        _consumer.Received += (model, ea) =>
        {
            try
            {
                var checkHealthResult = _healthCheckService.CheckHealthAsync().Result;

                var result = new HealthCheckRabbitMQMessageModel
                {
                    Status = checkHealthResult.Status,
                    Duration = checkHealthResult.TotalDuration
                };

                foreach (var entry in checkHealthResult.Entries)
                {
                    var entryMessage = new StringBuilder();
                    entryMessage.Append(entry.Value.Status.ToString());

                    if (!string.IsNullOrEmpty(entry.Value.Description))
                    {
                        entryMessage.Append(" - ");
                        entryMessage.Append(entry.Value.Description);
                    }

                    if (entry.Value.Exception is not null)
                    {
                        entryMessage.Append(" - ");
                        entryMessage.Append(entry.Value.Exception.Message);
                    }

                    result.Entries.Add(entry.Key, entryMessage.ToString());
                }

                var responseMessage = JsonConvert.SerializeObject(result);
                var properties = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = properties.CorrelationId;

                var body = Encoding.UTF8.GetBytes(responseMessage);

                _channel.BasicPublish(exchange: "", routingKey: properties.ReplyTo, basicProperties: replyProps, body: body);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        };

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
