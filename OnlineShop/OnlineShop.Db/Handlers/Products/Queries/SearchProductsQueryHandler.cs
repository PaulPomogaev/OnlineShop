using MediatR;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Mapping;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ICacheService _cache;

       public SearchProductsQueryHandler(IProductQueryRepository productQueryRepository, ICacheService cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var query = request;
            var normalizedQuery = query.Query?.Trim().ToLowerInvariant() ?? "empty";
            var cacheKey = $"search_{normalizedQuery}";

            var cachedResults = await _cache.GetAsync<List<ProductDto>>(cacheKey);
            if (cachedResults != null)
            {
                return cachedResults;
            }

            var products = await _productQueryRepository.SearchEngineAsync(query.Query);
                        
            var productDtos = products.ToDtoList();

            await _cache.SetAsync(cacheKey, productDtos, TimeSpan.FromMinutes(2));

            return productDtos;
        }
    }
}
