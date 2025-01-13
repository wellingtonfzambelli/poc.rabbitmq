namespace poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;

public interface IRabbitMQService : IMessageService
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}