using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Services.Notifications.Messages.Commands;
using Microsoft.Extensions.Logging;

namespace DShop.Services.Notifications.Handlers
{
    public class SendEmailNotificationHandler : ICommandHandler<SendEmailNotification>
    {
        private readonly ILogger<SendEmailNotificationHandler> _logger;

        public SendEmailNotificationHandler(ILogger<SendEmailNotificationHandler> logger)
        {
            _logger = logger;
        }
        
        public Task HandleAsync(SendEmailNotification command, ICorrelationContext context)
        {
            _logger.LogInformation($"Sending an email message to: '{command.Email}'.");
            
            // Publish: EmailSent
            // Publish: SendEmailNotificationRejected

            return Task.CompletedTask;
        }
    }
}