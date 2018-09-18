using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Notifications.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("DShop Notifications Service");
    }
}