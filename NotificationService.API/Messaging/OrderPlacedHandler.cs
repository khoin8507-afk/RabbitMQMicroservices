using Messaging.Common.Events;
using NotificationService.Contracts.Messaging;

namespace NotificationService.API.Messaging
{
    public class OrderPlacedHandler : IOrderPlacedHandler
    {
        public Task HandleAsync(OrderPlacedEvent evt)
        {
            Console.WriteLine($"Sending email to {evt.CustomerEmail}");

            return Task.CompletedTask;
        }
    }
}