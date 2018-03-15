using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Notifications.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get() => Ok("DShop Notifications Service");
    }
}