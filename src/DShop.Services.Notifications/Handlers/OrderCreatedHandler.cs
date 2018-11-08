using DShop.Common.Handlers;
using DShop.Common.MailKit;
using DShop.Common.RabbitMq;
using DShop.Services.Notifications.Builders;
using DShop.Services.Notifications.Services;
using DShop.Services.Notifications.Templates;
using System.Threading.Tasks;
using DShop.Services.Notifications.Messages.Events;

namespace DShop.Services.Notifications.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly MailKitOptions _options;
        private readonly ICustomersService _customersService;
        private readonly IMessagesService _messagesService;

        public OrderCreatedHandler(
            MailKitOptions options, 
            ICustomersService customersService, 
            IMessagesService messagesService)
        {
            _options = options;
            _customersService = customersService;
            _messagesService = messagesService;
        }

        public async Task HandleAsync(OrderCreated @event, ICorrelationContext context)
        {
            var orderId = @event.Id.ToString("N");
            var customer = await _customersService.GetAsync(@event.CustomerId);
            var message = MessageBuilder
                .Create()
                .WithReceiver(customer.Email)
                .WithSender(_options.Email)
                .WithSubject(MessageTemplates.OrderCreatedSubject, orderId)
                .WithBody(MessageTemplates.OrderCreatedBody, customer.FirstName, customer.LastName, orderId)
                .Build();

            await _messagesService.SendAsync(message);
        }
    }
}
