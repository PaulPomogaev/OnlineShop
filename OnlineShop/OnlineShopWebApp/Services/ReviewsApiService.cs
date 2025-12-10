using Microsoft.Extensions.Configuration;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models.Reviews;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using System.Text.Json;

namespace OnlineShopWebApp.Services
{
    public class ReviewsApiService : IReviewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public ReviewsApiService(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }


        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"/api/Reviews/filter?productId={productId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Review>>();
            }

            return new List<Review>();
        }

        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"/api/Reviews/{reviewId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Review>();
            }

            return null;
        }

        public async Task<Review> AddReviewAsync(AddReviewRequest request)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.PostAsJsonAsync($"/api/Reviews", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Review>();
            }

            return null;
        }

        public async Task<ProductRatingDto> GetProductRatingAsync(int productId)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"/api/Product/{productId}/rating");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductRatingDto>();
            }

            return new ProductRatingDto { Rating = 0, ReviewCount = 0 };
        }

        public async Task<List<ProductRatingDtoWithId>> GetProductRatingsAsync(List<int> productIds)
        {
            if(productIds == null || productIds.Count == 0)
            {
                return new List<ProductRatingDtoWithId>();
            }

            var ids = string.Join(",", productIds.Distinct().Where(id => id > 0));
            var response = await _httpClient.GetAsync($"/api/Product/ratings?ids={ids}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ProductRatingDtoWithId>>();
                return result ?? new List<ProductRatingDtoWithId>();
            }

            return new List<ProductRatingDtoWithId>();
        }
    }
}
