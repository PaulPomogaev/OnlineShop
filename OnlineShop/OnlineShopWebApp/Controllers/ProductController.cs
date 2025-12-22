using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models.Reviews;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;

namespace OnlineShopWebApp.Controllers

{
    public class ProductController : Controller
    {
        private readonly IQueryHandler<GetProductByIdQuery, ProductDto?> _getProductByIdHandler;
        private readonly IReviewsApiService _reviewsApiService;
        private readonly UserManager<User> _userManager;

       public ProductController(IQueryHandler<GetProductByIdQuery, ProductDto?> getProductByIdHandler, IReviewsApiService reviewsApiService, UserManager<User> userManager)
        {
            _getProductByIdHandler = getProductByIdHandler;
            _reviewsApiService = reviewsApiService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var product = await _getProductByIdHandler.Handle(new GetProductByIdQuery(id));
            if (product == null)
            {
                return NotFound();
            }

            var reviews = await _reviewsApiService.GetReviewsByProductIdAsync(id);
            var rating = await _reviewsApiService.GetProductRatingAsync(id);

            var productViewModel = product.ToViewModel().WithReviews(reviews, rating);

            return View(productViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int productId, string text, int grade)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(text) || grade < 0 || grade > 5)
            {
                TempData["ErrorMessage"] = "Пожалуйста, заполните все поля корректно.";
                return RedirectToAction("Index", new { id = productId });
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Пользователь не найден.";
                    return RedirectToAction("Index", new { id = productId });
                }

                var reviewRequest = new AddReviewRequest
                {
                    ProductId = productId,
                    UserId = user.Id, 
                    Text = text.Trim(),
                    Grade = grade
                };

                var result = await _reviewsApiService.AddReviewAsync(reviewRequest);

                if (result != null)
                {
                    TempData["SuccessMessage"] = "Ваш отзыв успешно добавлен!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Не удалось добавить отзыв.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";
            }

            return RedirectToAction("Index", new { id = productId });
        }
    }
}
