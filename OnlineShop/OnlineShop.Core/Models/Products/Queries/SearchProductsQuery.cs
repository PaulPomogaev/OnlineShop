using OnlineShop.Core.Interfaces.Cqrs;

namespace OnlineShop.Core.Models.Products.Queries
{
    public record SearchProductsQuery(string query) : IQuery<List<ProductDto>>;

}
