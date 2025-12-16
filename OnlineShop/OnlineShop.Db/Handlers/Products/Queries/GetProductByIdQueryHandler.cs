using MediatR;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Mapping;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache; 

        public GetProductByIdQueryHandler(IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var query = request;
            var cacheKey = $"product_{query.Id}";

            if(_cache.TryGetValue(cacheKey, out ProductDto? cachedProduct))
            {
                return cachedProduct;
            }

            var product = await _productQueryRepository.GetByIdAsync(query.Id);
            if (product == null)
            {
               return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)).SetPriority(CacheItemPriority.Normal);

            var productDto = product.ToDto();

            _cache.Set(cacheKey, productDto, cacheOptions);

            return productDto;
        }
    }
}
