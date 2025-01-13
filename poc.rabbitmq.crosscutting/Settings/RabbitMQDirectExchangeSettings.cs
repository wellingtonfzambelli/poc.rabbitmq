namespace poc.rabbitmq.crosscutting.Settings;

public sealed class RabbitMQDirectExchangeSettings
{
    public string Queue { get; set; } = string.Empty;
    public string ExchangeName { get; set; } = string.Empty;
}