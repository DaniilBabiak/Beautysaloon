using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        try
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
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while trying to send customer to queue.");
        }
    }
}
