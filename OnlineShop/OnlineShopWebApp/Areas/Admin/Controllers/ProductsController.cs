using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;
using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductsController : Controller
    {
        private readonly IQueryHandler<GetAllProductsQuery, List<ProductDto>> _getAllProductsHandler;
        private readonly IQueryHandler<GetProductByIdQuery, ProductDto?> _getProductByIdHandler;
        private readonly ICommandHandler<CreateProductCommand, int> _createProductHandler;
        private readonly ICommandHandler<EditProductCommand, bool> _editProductHandler;
        private readonly ICommandHandler<DeleteProductCommand, bool> _deleteProductHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IQueryHandler<GetAllProductsQuery, List<ProductDto>> getAllProductsHandler, IQueryHandler<GetProductByIdQuery, ProductDto?> getProductByIdHandler, ICommandHandler<CreateProductCommand, int> createProductHandler, ICommandHandler<EditProductCommand, bool> editProductHandler, ICommandHandler<DeleteProductCommand, bool> deleteProductHandler, IWebHostEnvironment webHostEnvironment)
        {
            _getAllProductsHandler = getAllProductsHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _createProductHandler = createProductHandler;
            _editProductHandler = editProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _getAllProductsHandler.Handle(new GetAllProductsQuery());
           
            return View(products.ToViewModels());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _getProductByIdHandler.Handle(new GetProductByIdQuery(id));
            if (product == null)
            {
                return NotFound();
            }
                        
            return View(product.ToViewModel());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _getProductByIdHandler.Handle(new GetProductByIdQuery(id));

            if (product == null)
            {
                return NotFound();
            }
            return View(product.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
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

            var existingProduct = await _getProductByIdHandler.Handle(new GetProductByIdQuery(id));
            if (existingProduct == null)
            {
                return NotFound();
            }

            string photoPath = existingProduct.PhotoPath;

            List<string> imagePaths = existingProduct.ImagePaths ?? new List<string>();

            if (model.UploadedFile != null && model.UploadedFile.Length > 0)
            {
                photoPath =  await SaveImageAsync(model.UploadedFile);
            }

            if (model.UploadedFiles != null && model.UploadedFiles.Any())
            {
                foreach (var imageFile in model.UploadedFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile);
                        imagePaths.Add(imagePath);
                    }
                }
            }

            var command = new EditProductCommand(
                model.Id,
                model.Name,
                model.Cost,
                model.Description,
                photoPath,
                imagePaths
            );

            await _editProductHandler.Handle(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _deleteProductHandler.Handle(new DeleteProductCommand(id));
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string photoPath = "/img/whey-protein.jpg";
            var imagePaths = new List<string>();

            if (model.UploadedFile != null)
            {
                photoPath = await SaveImageAsync(model.UploadedFile);
            }

            if (model.UploadedFiles != null && model.UploadedFiles.Any())
            {
                foreach (var imageFile in model.UploadedFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile);
                        imagePaths.Add(imagePath);
                    }
                }
            }

            var command = new CreateProductCommand(
                model.Name,
                model.Cost,
                model.Description,
                photoPath,
                imagePaths
            );

            await _createProductHandler.Handle(command);
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
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
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/products/{fileName}";
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int productId, string imagePath)
        {
            var product = await _getProductByIdHandler.Handle(new GetProductByIdQuery(productId));
            if (product == null)
            {
                return NotFound();
            }

            if (product.ImagePaths != null && product.ImagePaths.Contains(imagePath))
            {
                var updatedImagePaths = product.ImagePaths.Where(p => p != imagePath).ToList();

                var command = new EditProductCommand(
                    productId,
                    product.Name,
                    product.Cost,
                    product.Description,
                    product.PhotoPath,
                    updatedImagePaths
                );

                await _editProductHandler.Handle(command);

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
        public async Task<IActionResult> SetAsMainImage(int productId, string imagePath)
        {
            var product = await _getProductByIdHandler.Handle(new GetProductByIdQuery(productId));
            if (product == null)
            {
                return NotFound();
            }
            var command = new EditProductCommand(
                productId,
                product.Name,
                product.Cost,
                product.Description,
                imagePath,
                product.ImagePaths
            );


            await _editProductHandler.Handle(command);

            return RedirectToAction("Edit", new { id = productId });
        }
    }
}
