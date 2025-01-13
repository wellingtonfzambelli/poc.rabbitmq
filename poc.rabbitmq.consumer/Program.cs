using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using poc.rabbitmq.consumer.Jobs;
using poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;
using poc.rabbitmq.crosscutting.Settings;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<UserConsumerJob>();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

builder.Services.Configure<DirectRabbitMQSettings>(builder.Configuration.GetSection("DirectRabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<DirectRabbitMQSettings>>().Value);

builder.Services.AddTransient<IRabbitMQService, RabbitMQService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();




using var host = builder.Build();
await host.RunAsync();