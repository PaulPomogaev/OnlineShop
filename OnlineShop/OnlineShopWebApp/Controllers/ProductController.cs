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
            var product = ProductJsonRepository.ReturnById(id);
            if(product == null)
            {
                return View(null);
            }

            return View(product);
        }
    }
}
