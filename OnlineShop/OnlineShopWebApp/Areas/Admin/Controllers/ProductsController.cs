using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;
using OnlineShop.Db.Repostories;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductsController : Controller
    {
        private readonly IProductQueryRepository _productQueryRepository; 
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductQueryRepository productQueryRepository, IProductCommandRepository productCommandRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productQueryRepository = productQueryRepository;
            _productCommandRepository = productCommandRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productQueryRepository.GetAll();
            return View(products.ToViewModels());
        }

        public IActionResult Detail(int id)
        {
            var product = _productQueryRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = product.ToViewModel();
            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            var product = _productQueryRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product.ToViewModel());
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id != model.Id)
            {
                ModelState.AddModelError("", "Несоответствие идентификаторов");
                return View(model);
            }

            var existingProduct = _productQueryRepository.GetById(model.Id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = model.Name;
            existingProduct.Cost = model.Cost;
            existingProduct.Description = model.Description;

            if (model.UploadedFile != null)
            {
                existingProduct.PhotoPath = SaveImage(model.UploadedFile);
            }

            if (model.UploadedFiles != null && model.UploadedFiles.Any())
            {
                existingProduct.ImagePaths ??= new List<string>();

                foreach (var imageFile in model.UploadedFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        var imagePath = SaveImage(imageFile);
                        existingProduct.ImagePaths.Add(imagePath);
                    }
                }
            }

            _productCommandRepository.Edit(existingProduct);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _productCommandRepository.Delete(id);
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
            var imagePaths = new List<string>();

            if (model.UploadedFile != null)
            {
                photoPath = SaveImage(model.UploadedFile);
            }

            if(model.UploadedFiles != null && model.UploadedFiles.Any())
            {
                foreach(var imageFile in model.UploadedFiles)
                {
                    if(imageFile.Length >0)
                    {
                        var imagePath = SaveImage(imageFile);
                        imagePaths.Add(imagePath); 
                    }
                }
            }

            var dbProduct = new Product
            {
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description,
                PhotoPath = photoPath,
                ImagePaths = imagePaths
            };

            _productCommandRepository.Add(dbProduct);
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

        [HttpPost]
        public IActionResult DeleteImage(int productId, string imagePath)
        {
            var product = _productQueryRepository.GetById(productId);
            if (product == null) return NotFound();

            if (product.ImagePaths != null && product.ImagePaths.Contains(imagePath))
            {
                product.ImagePaths.Remove(imagePath);
                _productCommandRepository.Edit(product);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            return RedirectToAction("Edit", new { id = productId });
        }

        [HttpPost]
        public IActionResult SetAsMainImage(int productId, string imagePath)
        {
            var product = _productQueryRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            product.PhotoPath = imagePath;
            _productCommandRepository.Edit(product);

            return RedirectToAction("Edit", new { id = productId });
        }
    }
}
