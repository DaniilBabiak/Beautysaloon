namespace BeautySaloon.API.RabbitMq;

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Uri { get; set; }
    public string CustomerQueueName { get; set; }
    public string UnusedImagesRequestQueueName { get; set; }
}
