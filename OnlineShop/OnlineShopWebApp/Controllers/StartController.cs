using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class StartController : Controller
    {
        public string Hello()
        {
            var now = DateTime.Now;

            if (now.Hour >= 6 && now.Hour < 12)
                return "Доброе утро";

            if (now.Hour < 18)
                return "Добрый день";

            if (now.Hour < 24)
                return "Добрый вечер";

            return "Доброй ночи";
        }
    }
}
