using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        [Route("calculator/index/{num1?}/{num2?}")] 
        public string Index(double? num1 = 0, double? num2 = 0)
        {
            
            var result = num1 + num2;

            return $"{num1} + {num2} = {result}";
        }
    }
}
