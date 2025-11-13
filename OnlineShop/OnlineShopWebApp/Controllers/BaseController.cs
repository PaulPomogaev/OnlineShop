using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(User.Identity.Name))
            {
                return User.Identity.Name;
            }

            var sessionId = HttpContext.Session.Id;

            if (string.IsNullOrEmpty(sessionId))
            {
                HttpContext.Session.SetString("__init", "1");
                sessionId = HttpContext.Session.Id;
            }

            return $"guest_{sessionId}";
        }
    }
}
