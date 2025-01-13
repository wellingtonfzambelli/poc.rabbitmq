using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;

namespace poc.rabbitmq.consumer.Jobs;

internal sealed class UserConsumerJob : BackgroundService
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<UserConsumerJob> _logger;

    public UserConsumerJob(IRabbitMQService rabbitMQService, ILogger<UserConsumerJob> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserConsumerJob started.");
        await _rabbitMQService.ConsumeAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("UserConsumerJob is stopping...");
        await base.StopAsync(cancellationToken);
    }
}