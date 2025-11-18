using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products.ToViewModels());
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            var viewModel = product.ToViewModel();
            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product.ToViewModel());
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

            if (model.UploadedFile != null)
            {
                existingProduct.PhotoPath = SaveImage(model.UploadedFile);
            }
            
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

            string photoPath = "/img/whey-protein.jpg";

            if (model.UploadedFile != null)
            {
                photoPath = SaveImage(model.UploadedFile);
            }
            
            var dbProduct = new Product
            {
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description,
                PhotoPath = photoPath
            };

            _productRepository.Add(dbProduct);
            return RedirectToAction("Index");
        }

        private string SaveImage(IFormFile imageFile)
        {
            string productImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            if (!Directory.Exists(productImagesPath))
            {
                Directory.CreateDirectory(productImagesPath);
            }

            var fileName = Guid.NewGuid() + "." + imageFile.FileName.Split('.').Last();
            var filePath = Path.Combine(productImagesPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return $"/images/products/{fileName}";
        }
    }
}
