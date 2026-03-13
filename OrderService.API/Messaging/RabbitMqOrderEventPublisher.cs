using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Messaging.Common.Events;
using Messaging.Common.Options;
using Messaging.Common.Topology;
using OrderService.Contracts.Messaging;

namespace OrderService.API.Messaging
{
    public class RabbitMqOrderEventPublisher : IOrderEventPublisher
    {
        private readonly IModel _channel;
        private readonly RabbitMqOptions _options;

        public RabbitMqOrderEventPublisher(IModel channel, IOptions<RabbitMqOptions> options)
        {
            _channel = channel;
            _options = options.Value;

            RabbitTopology.EnsureAll(_channel, _options);
        }

        public Task PublishOrderPlacedAsync(OrderPlacedEvent evt, string? correlationId = null)
        {
            evt.CorrelationId = correlationId ?? evt.CorrelationId;

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));

            _channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: "order.placed",
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }
    }
}