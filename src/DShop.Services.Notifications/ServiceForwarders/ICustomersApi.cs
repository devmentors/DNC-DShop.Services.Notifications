using DShop.Services.Notifications.Dtos;
using RestEase;
using System;
using System.Threading.Tasks;

namespace DShop.Services.Notifications.ServiceForwarders
{
    public interface ICustomersApi
    {
        [AllowAnyStatusCode]
        [Get("customers/{id}")]
        Task<CustomerDto> GetAsync([Path] Guid id);
    }
}
