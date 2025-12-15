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
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, List<ProductDto>>
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public SearchProductsQueryHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<List<ProductDto>> Handle(SearchProductsQuery query, CancellationToken cancellationToken = default)
        {
            var products = await _productQueryRepository.SearchEngineAsync(query.query);
            return products.ToDtoList();
        }
    }
}
