using MimeKit;

namespace DShop.Services.Notifications.Builders
{
    public class MessageBuilder : IMessageBuilder
    {
        private readonly MimeMessage _message;

        private MessageBuilder()
        {
            _message = new MimeMessage();
        }

        public static IMessageBuilder Create()
            => new MessageBuilder();

        IMessageBuilder IMessageBuilder.WithSender(string senderEmail)
        {
            _message.From.Add(new MailboxAddress(senderEmail));
            return this;
        }

        IMessageBuilder IMessageBuilder.WithReceiver(string receiverEmail)
        {
            _message.To.Add(new MailboxAddress(receiverEmail));
            return this;
        }

        IMessageBuilder IMessageBuilder.WithSubject(string subject)
        {
            _message.Subject = subject;
            return this;
        }

        IMessageBuilder IMessageBuilder.WithBody(string body)
        {
            _message.Body = new TextPart("plain")
            {
                Text = body
            };
            return this;
        }

        MimeMessage IMessageBuilder.Build()
            => _message;
    }
}
