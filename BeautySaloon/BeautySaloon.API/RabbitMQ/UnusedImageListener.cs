using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Helpers;
using BeautySaloon.API.RabbitMq.Models;
using BeautySaloon.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace BeautySaloon.API.RabbitMq;

public class UnusedImageListener : BackgroundService
{
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private RabbitMQ.Client.IModel _channel;
    private EventingBasicConsumer _consumer;
    private readonly RabbitMQSettings _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceScope _scope;
    private readonly BeautySaloonContext _context;

    public UnusedImageListener(IOptions<RabbitMQSettings> options, IServiceScopeFactory scopeFactory)
    {
        _options = options.Value;
        _scopeFactory = scopeFactory;
        _scopeFactory = scopeFactory;
        _scope = scopeFactory.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<BeautySaloonContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connectionFactory = new ConnectionFactory()
                {
                    HostName = _options.HostName,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    Uri = new Uri(_options.Uri)
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
        _channel.QueueDeclare(queue: _options.UnusedImagesRequestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.BasicConsume(queue: _options.UnusedImagesRequestQueueName, autoAck: true, consumer: _consumer);

        _consumer.Received += async (model, ea) =>
        {
            try
            {
                var requestBody = ea.Body;
                var requestJson = Encoding.UTF8.GetString(requestBody.ToArray());
                var request = JsonConvert.DeserializeObject<List<MinioLocation>>(requestJson);
                
                var response = new List<MinioLocation>();

                if (request is not null || request.Any())
                {
                    response = await GetUnusedObjectsAsync(request);
                }                

                var responseMessage = JsonConvert.SerializeObject(response);
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

    public async Task<List<MinioLocation>> GetUnusedObjectsAsync(List<MinioLocation> minioLocations)
    {
        var getAllCategoriesTask = _context.ServiceCategories
                                            .Where(c => c.ImageBucket != null && c.ImageFileName != null)
                                            .Select(c => new MinioLocation
                                            {
                                                BucketName = c.ImageBucket,
                                                FileName = c.ImageFileName
                                            })
                                            .ToListAsync();

        var getAllBestWorksTask = _context.BestWorks
                                            .Where(b => b.ImageBucket != null && b.ImageFileName != null)
                                            .Select(b => new MinioLocation
                                            {
                                                BucketName = b.ImageBucket,
                                                FileName = b.ImageFileName
                                            })
                                            .ToListAsync();

        await Task.WhenAll(getAllCategoriesTask, getAllBestWorksTask);

        var allObjectsInDb = getAllCategoriesTask.Result.Concat(getAllBestWorksTask.Result);

        var allObjectsInMinio = minioLocations;

        var unusedObjects = allObjectsInMinio
    .Where(minioObj => !allObjectsInDb.Any(dbObj =>
        dbObj.BucketName == minioObj.BucketName && dbObj.FileName == minioObj.FileName))
    .ToList();

        return unusedObjects;
    }
}

