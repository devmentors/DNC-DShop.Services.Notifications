using MimeKit;
using System.Threading.Tasks;

namespace DShop.Services.Notifications.Services
{
    public interface IMessagesService
    {
        Task SendAsync(MimeMessage message);
    }
}
