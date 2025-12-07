using OnlineShop.Core.Models.Reviews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Interfaces
{
    public interface IReviewsApiService
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<Review> AddReviewAsync(AddReviewRequest request);
        Task<ProductRatingDto> GetProductRatingAsync(int productId);
    }
}