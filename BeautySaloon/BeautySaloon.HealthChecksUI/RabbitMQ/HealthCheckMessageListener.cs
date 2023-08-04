using BeautySaloon.Shared;
using HealthChecks.UI.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BeautySaloon.HealthChecksUI.RabbitMQ;

public class HealthCheckMessageListener : BackgroundService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;

    private readonly IModel _apiHealthChannel;
    private readonly IModel _identityHealthChannel;

    private readonly EventingBasicConsumer _apiHealthConsumer;
    private readonly EventingBasicConsumer _identityHealthConsumer;

    private readonly IMemoryCache _memoryCache;
    private readonly RabbitMQSettings _options;

    public HealthCheckMessageListener(IMemoryCache memoryCache, IOptions<RabbitMQSettings> options)
    {
        _options = options.Value;
        _memoryCache = memoryCache;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Uri = new Uri(_options.Uri)
        };

        _connection = _connectionFactory.CreateConnection();

        _apiHealthChannel = _connection.CreateModel();
        _apiHealthChannel.QueueDeclare(queue: RabbitMQQueuesConfig.APIHealthQueue,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
        _apiHealthChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        _apiHealthConsumer = new EventingBasicConsumer(_apiHealthChannel);

        _identityHealthChannel = _connection.CreateModel();
        _identityHealthChannel.QueueDeclare(queue: RabbitMQQueuesConfig.IdentityHealthQueue,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
        _identityHealthChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        _identityHealthConsumer = new EventingBasicConsumer(_identityHealthChannel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _apiHealthConsumer.Received += (model, args) =>
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            var healthReport = JsonConvert.DeserializeObject<UIHealthReport>(message);

            _memoryCache.Set("api_healthcheck_result", healthReport);

            _apiHealthChannel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);

        };

        _apiHealthChannel.BasicConsume(queue: RabbitMQQueuesConfig.APIHealthQueue,
                              autoAck: false,
                              consumer: _apiHealthConsumer);

        _identityHealthConsumer.Received += (model, args) =>
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            var healthReport = JsonConvert.DeserializeObject<UIHealthReport>(message);

            _memoryCache.Set("identity_healthcheck_result", healthReport);


            _identityHealthChannel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);

        };

        _identityHealthChannel.BasicConsume(queue: RabbitMQQueuesConfig.IdentityHealthQueue,
                              autoAck: false,
                              consumer: _identityHealthConsumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        base.Dispose();
        _connection?.Dispose();
    }
}
