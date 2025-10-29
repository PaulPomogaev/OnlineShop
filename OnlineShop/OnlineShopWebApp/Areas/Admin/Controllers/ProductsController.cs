using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            var viewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Cost = p.Cost,
                Description = p.Description
            }).ToList();

            return View(viewModels);
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = "img/whey-protein.jpg"
            };
            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                        
            var existingProduct = _productRepository.GetById(model.Id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = model.Name;
            existingProduct.Cost = model.Cost;
            existingProduct.Description = model.Description;

            _productRepository.Edit(existingProduct);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dbProduct = new Product
            {
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description
            };

            _productRepository.Add(dbProduct);
            return RedirectToAction("Index");
        }
    }
}
