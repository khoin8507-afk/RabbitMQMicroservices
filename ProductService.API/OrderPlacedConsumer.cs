using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Messaging.Common.Events;
using ProductService.Contracts.Messaging;

namespace ProductService.API.Messaging
{
    public class OrderPlacedConsumer
    {
        private readonly IModel _channel;
        private readonly IOrderPlacedHandler _handler;

        public OrderPlacedConsumer(IModel channel, IOrderPlacedHandler handler)
        {
            _channel = channel;
            _handler = handler;
        }

        public void Start()
        {
            // tạo queue nếu chưa tồn tại
            _channel.QueueDeclare(
                queue: "product.order_placed",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, ea) =>
            {
                var evt = JsonSerializer.Deserialize<OrderPlacedEvent>(ea.Body.Span);

                if (evt != null)
                {
                    await _handler.HandleAsync(evt);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: "product.order_placed",
                autoAck: false,
                consumer: consumer);
        }
    }
}