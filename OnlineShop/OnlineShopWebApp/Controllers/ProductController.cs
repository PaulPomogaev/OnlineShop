using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Models;
using System.Text;

namespace OnlineShopWebApp.Controllers

{
    public class ProductController : Controller
    {
        public IActionResult Index(int id)
        {
            var product = ProductJsonRepository.GetAll().FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return Content($"Товар с ID {id} не найден");
            }

            var stringBuilder = new StringBuilder();
            
                stringBuilder.AppendLine($"{product.Id}");
                stringBuilder.AppendLine($"{product.Name}");
                stringBuilder.AppendLine($"{product.Cost}");
                stringBuilder.AppendLine($"{product.Description}");
                stringBuilder.AppendLine();
            
            return Content(stringBuilder.ToString(), "text/plain");
        }
    }
}
