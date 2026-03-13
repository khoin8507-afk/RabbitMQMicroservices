using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Messaging.Common.Events;
using NotificationService.Contracts.Messaging;

namespace NotificationService.API.Messaging
{
    public class OrderPlacedConsumer
    {
        private readonly IModel _channel;
        private readonly IOrderPlacedHandler _handler;

        public OrderPlacedConsumer(
            IModel channel,
            IOrderPlacedHandler handler)
        {
            _channel = channel;
            _handler = handler;
        }

        public void Start()
        {
            // tạo queue nếu chưa tồn tại
            _channel.QueueDeclare(
                queue: "notification.order_placed",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var message = ea.Body.ToArray();
                    var evt = JsonSerializer.Deserialize<OrderPlacedEvent>(message);

                    if (evt != null)
                    {
                        await _handler.HandleAsync(evt);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(
                queue: "notification.order_placed",
                autoAck: false,
                consumer: consumer);

            Console.WriteLine("NotificationService is listening for OrderPlacedEvent...");
        }
    }
}