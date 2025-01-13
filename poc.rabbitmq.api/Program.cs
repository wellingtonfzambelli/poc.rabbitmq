using Microsoft.Extensions.Options;
using poc.rabbitmq.crosscutting.Domain;
using poc.rabbitmq.crosscutting.MessageBroker.RabbitMQ;
using poc.rabbitmq.crosscutting.Settings;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

builder.Services.Configure<DirectRabbitMQSettings>(builder.Configuration.GetSection("DirectRabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<DirectRabbitMQSettings>>().Value);

builder.Services.AddTransient<IRabbitMQService, RabbitMQService>();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/user", async (User user, IRabbitMQService reabbitMQService, CancellationToken ct) =>
{
    try
    {
        await reabbitMQService.ProduceAsync(JsonSerializer.Serialize(user), ct);
        return Results.Ok("6");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error by producing the user: {ex.Message}");
    }
})
.WithOpenApi();

app.Run();