namespace poc.rabbitmq.crosscutting.MessageBroker;

public interface IMessageService
{
    Task ProduceAsync(string message, CancellationToken ct);
}