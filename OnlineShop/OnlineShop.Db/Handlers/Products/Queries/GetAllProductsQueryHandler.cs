using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Mapping;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public GetAllProductsQueryHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken = default)
        {
            var products = await _productQueryRepository.GetAllAsync();
            return products.ToDtoList();
        }
    }
}
