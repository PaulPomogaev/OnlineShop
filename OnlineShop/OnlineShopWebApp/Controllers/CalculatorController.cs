using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(double? num1 = 0, double? num2 = 0)
        {
            double n1 = num1 ?? 0;
            double n2 = num2 ?? 0;
            var result = n1 + n2;

            return $"{n1} + {n2} = {result}";
        }
    }
}
