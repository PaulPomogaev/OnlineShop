using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalculatorController : Controller
    {
        public string Index(double? a = 0, double? b = 0, string c = "+")
        {
            double num1 = a ?? 0;
            double num2 = b ?? 0;
            if(c != "+" && c != "-" && c != "*" && c != "/")
            {
                return "Ошибка, допускается использование только данных символов: + или - или * или /. Например, ?a=5&b=2&c=/";
            }

            if(c == "/" && num2 == 0)
            {
                return "Ошибка, делай что хочешь, но на ноль дельить нельзя";
            }

            double result = c switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" => num1 / num2,
                _ => num1 + num2
            };

            return $"{num1} {c} {num2} = {result}";

        }
    }
}
