using OnlineShop.Core.Interfaces.Cqrs;

namespace OnlineShop.Core.Models.Products.Queries
{
    public record GetAllProductsQuery : IQuery<List<ProductDto>>;

}
