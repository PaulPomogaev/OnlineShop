using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Text;

namespace OnlineShopWebApp.Controllers

{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
           _productRepository = productRepository;
        }

        public IActionResult Index(int id)
        {
            var product = _productRepository.GetById(id);
            if(product == null)
            {
                return NotFound();
            }
            var productViewModel = product.ToViewModel();
            return View(productViewModel);
        }
    }
}
