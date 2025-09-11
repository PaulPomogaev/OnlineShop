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
            
                stringBuilder.AppendLine($"Id{product.Id}");
                stringBuilder.AppendLine($"Name{product.Name}");
                stringBuilder.AppendLine($"Cost{product.Cost}");
                stringBuilder.AppendLine($"Cost{product.Description}");
                stringBuilder.AppendLine();
            
            return Content(stringBuilder.ToString(), "text/plain");
        }
    }
}
