using DShop.Services.Notifications.Dto;
using RestEase;
using System;
using System.Threading.Tasks;

namespace DShop.Services.Notifications.Services
{
    public interface ICustomersService
    {
        [AllowAnyStatusCode]
        [Get("customers/{id}")]
        Task<CustomerDto> GetAsync([Path] Guid id);
    }
}
