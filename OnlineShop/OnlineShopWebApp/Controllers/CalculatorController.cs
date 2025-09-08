using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(double? num1 = 0, double? num2 = 0, string operation = "+")
        {
            double n1 = num1 ?? 0;
            double n2 = num2 ?? 0;

            if(operation != "+" && operation != "-" && operation != "*")
            {
                return "Ошибка, допустимо использование только трёх символов: + или - или *. Например, /calculator/index/1/3/+";
            }

            double result = operation switch
            {
                "+" => n1 + n2,
                "-" => n1 - n2,
                "*" => n1 * n2,
                _ => n1 + n2
            };

            return $"{n1} {operation} {n2} = {result}";
        }
    }
}
