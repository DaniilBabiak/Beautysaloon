using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Serilog;
using BeautySaloon.Shared;
using Newtonsoft.Json;
using System.Text;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;

namespace BeautySaloon.API.RabbitMq;

public class CustomerListener : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private EventingBasicConsumer _consumer;
    private readonly IServiceScopeFactory _scopeFactory;

    public CustomerListener(IOptions<RabbitMQSettings> settings, IServiceScopeFactory scopeFactory)
    {
        _settings = settings.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
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
        _channel.QueueDeclare(queue: _settings.CustomerQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.BasicConsume(queue: _settings.CustomerQueueName, autoAck: true, consumer: _consumer);

        _consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());

                var customer = JsonConvert.DeserializeObject<Customer>(body);

                using var scope = _scopeFactory.CreateScope();
                var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();

                Customer existingCustomer;

                try
                {
                    existingCustomer = await customerService.GetCustomerAsync(customer.Id);
                }
                catch
                {
                    existingCustomer = null;
                }


                if (existingCustomer is null)
                {
                    await customerService.CreateCustomerAsync(customer);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        };

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
