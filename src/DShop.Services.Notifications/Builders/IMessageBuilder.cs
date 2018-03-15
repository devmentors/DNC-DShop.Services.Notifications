using MimeKit;

namespace DShop.Services.Notifications.Builders
{
    public interface IMessageBuilder
    {
        IMessageBuilder WithSender(string senderEmail);
        IMessageBuilder WithReceiver(string receiverEmail);
        IMessageBuilder WithSubject(string subject);
        IMessageBuilder WithBody(string body);
        MimeMessage Build();
    }
}
