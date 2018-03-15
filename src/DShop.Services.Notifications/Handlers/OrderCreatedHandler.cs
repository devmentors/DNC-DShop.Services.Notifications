using DShop.Common.Handlers;
using DShop.Common.MailKit;
using DShop.Common.RabbitMq;
using DShop.Messages.Events.Orders;
using DShop.Services.Notifications.Builders;
using DShop.Services.Notifications.ServiceForwarders;
using DShop.Services.Notifications.Services;
using System.Threading.Tasks;

namespace DShop.Services.Notifications.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly MailKitOptions _options;
        private readonly ICustomersApi _customersApi;
        private readonly IMessagesService _messagesService;

        public OrderCreatedHandler(
            MailKitOptions options, 
            ICustomersApi customersApi, 
            IMessagesService messagesService)
        {
            _options = options;
            _customersApi = customersApi;
            _messagesService = messagesService;
        }

        public async Task HandleAsync(OrderCreated @event, ICorrelationContext context)
        {
            var customer = await _customersApi.GetAsync(@event.CustomerId);

            var message = MessageBuilder
                .Create()
                .WithReceiver(customer.Email)
                .WithSender(_options.Email)
                .WithSubject("Order created")
                .WithBody("Order created")
                .Build();

            await _messagesService.SendAsync(message);
        }
    }
}
