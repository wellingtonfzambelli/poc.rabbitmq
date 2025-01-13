namespace poc.rabbitmq.crosscutting.Settings;

public sealed class DirectRabbitMQSettings
{
    public string QueueName { get; set; } = string.Empty;
    public string ExchangeName { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
}