using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Controllers
{
    public class CalcController : Controller
    {
        public string Index(double a = 0, double b = 0, string c = "+")
        {
            
            if(c != "+" && c != "-" && c != "*" && c != "/")
            {
                return "Ошибка, допускается использование только данных символов: + или - или * или /. Например, ?a=5&b=2&c=/";
            }

            if(c == "/" && b == 0)
            {
                return "Ошибка! Делай что хочешь, но на ноль дельить нельзя!";
            }

            double result = c switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => a + b
            };

            return $"{a} {c} {b} = {result}";

        }
    }
}
