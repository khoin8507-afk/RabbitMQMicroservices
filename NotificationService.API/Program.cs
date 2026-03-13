using Messaging.Common.Options;
using Messaging.Common.Extensions;
using NotificationService.Contracts.Messaging;
using NotificationService.API.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

var mq = builder.Configuration
    .GetSection("RabbitMq")
    .Get<RabbitMqOptions>();

builder.Services.AddRabbitMq(
    mq.HostName,
    mq.UserName,
    mq.Password,
    mq.VirtualHost
);

builder.Services.AddSingleton<IOrderPlacedHandler, OrderPlacedHandler>();
builder.Services.AddSingleton<OrderPlacedConsumer>();

var app = builder.Build();

// Start RabbitMQ Consumer
var consumer = app.Services.GetRequiredService<OrderPlacedConsumer>();
consumer.Start();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();