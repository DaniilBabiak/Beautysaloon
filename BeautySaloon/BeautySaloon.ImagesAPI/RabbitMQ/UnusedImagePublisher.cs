using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using Microsoft.Extensions.Options;
using BeautySaloon.Shared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Runtime;
using BeautySaloon.ImagesAPI.Services;
using BeautySaloon.ImagesAPI.Models;

namespace BeautySaloon.ImagesAPI.RabbitMQ;

public class UnusedImagePublisher : BackgroundService
{
    private readonly RabbitMQSettings _options;
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private EventingBasicConsumer _consumer;
    private readonly IImageService _imageService;
    public UnusedImagePublisher(IOptions<RabbitMQSettings> options, IImageService imageService)
    {
        _options = options.Value;
        _connectionFactory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Uri = new Uri(_options.Uri)
        };
        _imageService = imageService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
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

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ConnectAndExecute(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(queue: _options.UnusedImagesRequestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: _options.UnusedImagesReplyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var correlationId = Guid.NewGuid().ToString();

        var properties = _channel.CreateBasicProperties();
        properties.ReplyTo = _options.UnusedImagesReplyQueueName;
        properties.CorrelationId = correlationId;

        var request = await _imageService.GetAllImagesAsync();
        var requestJson = JsonConvert.SerializeObject(request);
        var body = Encoding.UTF8.GetBytes(requestJson);

        _channel.BasicPublish(exchange: "", routingKey: _options.UnusedImagesRequestQueueName, basicProperties: properties, body: body);

        _channel.BasicConsume(queue: _options.UnusedImagesReplyQueueName, autoAck: true, consumer: _consumer);

        _consumer.Received += async (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var responseJson = Encoding.UTF8.GetString(ea.Body.ToArray());

                var response = JsonConvert.DeserializeObject<List<MinioLocation>>(responseJson);

                if (response is not null)
                {
                    foreach (var location in response)
                    {
                        await _imageService.DeleteImageAsync(location);
                    }
                }
            }
        };
    }
}
