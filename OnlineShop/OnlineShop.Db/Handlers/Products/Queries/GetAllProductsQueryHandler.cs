using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Mapping;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache;

        public GetAllProductsQueryHandler(IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken = default)
        {
            const string cacheKey = "all_products";

            if (_cache.TryGetValue(cacheKey, out List<ProductDto> cachedProducts))
            {
                return cachedProducts;
            }

            var products = await _productQueryRepository.GetAllAsync();
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)).SetPriority(CacheItemPriority.High);

            var productDtos = products.ToDtoList();

            _cache.Set(cacheKey, productDtos, cacheOptions);

            return productDtos;
        }
    }
}
