using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;
using OnlineShop.Db.Repostories;
using MediatR;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
           
            return View(products.ToViewModels());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
            {
                return NotFound();
            }
                        
            return View(product.ToViewModel());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));

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

            var existingProduct = await _mediator.Send(new GetProductByIdQuery(id));
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

            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
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
            try
            {
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

                await _mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при создании товара: {ex.Message}");
                return View(model);
            }
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
            var product = await _mediator.Send(new GetProductByIdQuery(productId));
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

                await _mediator.Send(command);

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
            var product = await _mediator.Send(new GetProductByIdQuery(productId));
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


            await _mediator.Send(command);

            return RedirectToAction("Edit", new { id = productId });
        }
    }
}
