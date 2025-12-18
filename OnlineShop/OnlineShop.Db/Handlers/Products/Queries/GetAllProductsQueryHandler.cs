using MediatR;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Mapping;
using OnlineShop.Core.Interfaces;


namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ICacheService _cache;

        public GetAllProductsQueryHandler(IProductQueryRepository productQueryRepository, ICacheService cache)
        {
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var query = request;
            var cacheKey = Constants.AllProducts;

            var cachedProducts = await _cache.GetAsync<List<ProductDto>>(cacheKey);
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = await _productQueryRepository.GetAllAsync();
            
            var productDtos = products.ToDtoList();

            await _cache.SetAsync(cacheKey, productDtos, TimeSpan.FromMinutes(5));

            return productDtos;
        }
    }
}
