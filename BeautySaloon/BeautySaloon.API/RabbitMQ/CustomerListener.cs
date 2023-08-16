﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Serilog;
using BeautySaloon.Shared;
using Newtonsoft.Json;
using System.Text;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.RabbitMQ;

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
                using var context = scope.ServiceProvider.GetRequiredService<BeautySaloonContext>();
                var existingCustomer = await context.Customers.FindAsync(customer.Id);

                if (existingCustomer is null)
                {
                    // Создание нового объекта Customer, так как объект с указанным id не найден
                    context.Customers.Add(customer);
                }
                else
                {
                    existingCustomer.Name = customer.Name;
                    // Обновление других свойств
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        };

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}