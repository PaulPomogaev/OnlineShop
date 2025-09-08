using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(double? num1 = 0, double? num2 = 0, string operation = "+")
        {
            return View();
        }
    }
}
