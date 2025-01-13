namespace poc.rabbitmq.crosscutting.Settings;

public sealed class RabbitMQSettings
{
    public string VHost { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}