using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Mapping;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache;

       public SearchProductsQueryHandler(IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(SearchProductsQuery query, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"search_{query.query?.Trim().ToLowerInvariant() ?? "empty"}";

            if (_cache.TryGetValue(cacheKey, out List<ProductDto> cachedResults))
            {
                return cachedResults;
            }

            var products = await _productQueryRepository.SearchEngineAsync(query.query);

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)).SetPriority(CacheItemPriority.Low);

            var productDtos = products.ToDtoList();

            _cache.Set(cacheKey, productDtos, cacheOptions);

            return productDtos;
        }
    }
}
