using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return UserIdHelper.GetUserId(HttpContext);
        }
    }
}
