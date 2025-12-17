using MediatR;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Mapping;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ICacheService _cache; 

        public GetProductByIdQueryHandler(IProductQueryRepository productQueryRepository, ICacheService cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var query = request;
            var cacheKey = $"product_{query.Id}";

            var cachedProduct = await _cache.GetAsync<ProductDto?>(cacheKey);
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            var product = await _productQueryRepository.GetByIdAsync(query.Id);
            if (product == null)
            {
               return null;
            }
                       

            var productDto = product.ToDto();

            await _cache.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(10));

            return productDto;
        }
    }
}
