using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;
using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Db.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Queries
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public GetProductByIdQueryHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default)
        {
            var product = await _productQueryRepository.GetByIdAsync(query.id);
            return product?.ToDto();
        }
    }
}
