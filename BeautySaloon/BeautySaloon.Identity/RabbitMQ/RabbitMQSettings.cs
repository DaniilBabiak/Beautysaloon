﻿namespace BeautySaloon.Identity.RabbitMQ;

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Uri { get; set; }
    public string CustomerQueueName { get; set; }
}
