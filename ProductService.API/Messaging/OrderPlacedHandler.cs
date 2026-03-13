using Messaging.Common.Events;
using ProductService.Contracts.Messaging;

namespace ProductService.API.Messaging
{
    public class OrderPlacedHandler : IOrderPlacedHandler
    {
        public Task HandleAsync(OrderPlacedEvent evt)
        {
            Console.WriteLine($"Reducing stock for order: {evt.OrderId}");

            return Task.CompletedTask;
        }
    }
}