using Microsoft.AspNetCore.Mvc;
using Messaging.Common.Events;
using OrderService.Contracts.Messaging;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderEventPublisher _publisher;

        public OrderController(IOrderEventPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var evt = new OrderPlacedEvent
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "John",
                CustomerEmail = "john@email.com",
                PhoneNumber = "123456789",
                TotalAmount = 100
            };

            await _publisher.PublishOrderPlacedAsync(evt);

            return Ok("Order created and event sent to RabbitMQ");
        }
    }
}