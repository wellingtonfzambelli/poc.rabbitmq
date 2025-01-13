using Microsoft.Extensions.Logging;
using poc.rabbitmq.crosscutting.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;

public sealed class RabbitMQService : IRabbitMQService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQService> _logger;


    public RabbitMQService(RabbitMQSettings settings, ILogger<RabbitMQService> logger)
    {
        _settings = settings;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _settings.Server,
            VirtualHost = _settings.VHost,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _logger = logger;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        using (IConnection connection = _connectionFactory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            string exchangeName = "my-direct-exchange";
            string queueName = "my-direct-queue";
            string routingKey = "my-routing-key";

            channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true);
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
            string exchangeName = "my-direct-exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true);

            string queueName = "my-direct-queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            // Vinculate queue to exchange with a routing key
            string routingKey = "my-routing-key";
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            // Publishh a message in the exchange with the same routing key
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey, // Should correspond the same routing key used in the binding
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Message sent: {message}");
        }
    }
}