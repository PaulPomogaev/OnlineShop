using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(double num1 = 0, double num2 = 0, string operation = "+")
        {
            
            if(operation != "+" && operation != "-" && operation != "*")
            {
                return "Ошибка, допустимо использование только трёх символов: + или - или *. Например, /calculator/index/1/3/+";
            }

            double result = operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                _ => num1 + num2
            };

            return $"{num1} {operation} {num2} = {result}";
        }
    }
}
