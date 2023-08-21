using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace BeautySaloon.Identity.RabbitMQ;

public class CustomerPublisher
{
    private readonly RabbitMQSettings _options;
    private readonly ConnectionFactory _connectionFactory;
    public CustomerPublisher(IOptions<RabbitMQSettings> options)
    {
        _options = options.Value;
        _connectionFactory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Uri = new Uri(_options.Uri)
        };
    }

    public void Enqueue(object customer)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetryForever(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, context) =>
                {
                    Log.Error(exception, "Error while sending customer to resource API.");
                })
            .Execute(() =>
            {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _options.CustomerQueueName,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                var message = JsonConvert.SerializeObject(customer);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty,
                                      routingKey: _options.CustomerQueueName,
                                      basicProperties: null,
                                      body: body);

                return Task.CompletedTask;
            });
    }
}
