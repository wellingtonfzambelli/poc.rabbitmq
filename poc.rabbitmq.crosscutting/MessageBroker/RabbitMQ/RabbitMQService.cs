using Microsoft.Extensions.Logging;
using poc.rabbitmq.crosscutting.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;

public sealed class RabbitMQService : IRabbitMQService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly DirectRabbitMQSettings _directSettings;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(RabbitMQSettings settings, DirectRabbitMQSettings directSettings, ILogger<RabbitMQService> logger)
    {
        _directSettings = directSettings;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = settings.Server,
            VirtualHost = settings.VHost,
            UserName = settings.UserName,
            Password = settings.Password
        };

        _logger = logger;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        using (IConnection connection = _connectionFactory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            string exchangeName = _directSettings.ExchangeName;
            string queueName = _directSettings.QueueName;
            string routingKey = _directSettings.RoutingKey;

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");
            };

            // Iniciar o consumo da fila
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Waiting messages. Press [Enter] to leave.");
            Console.ReadLine();
        }
    }

    public async Task ProduceAsync(string message, CancellationToken cancellationToken)
    {
        using (IConnection connection = _connectionFactory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _directSettings.ExchangeName, type: "direct", durable: true);
            channel.QueueDeclare(queue: _directSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: _directSettings.QueueName, exchange: _directSettings.ExchangeName, routingKey: _directSettings.RoutingKey); // Vinculate queue to exchange with a routing key

            // Publish a message in the exchange with the same routing key
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: _directSettings.ExchangeName,
                routingKey: _directSettings.RoutingKey, // Should correspond the same routing key used in the binding
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Message sent: {message}");
        }
    }
}