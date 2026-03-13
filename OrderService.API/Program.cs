using Messaging.Common.Extensions;
using Messaging.Common.Options;
using OrderService.API.Messaging;
using OrderService.Contracts.Messaging;

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

builder.Services.AddSingleton<IOrderEventPublisher, RabbitMqOrderEventPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();