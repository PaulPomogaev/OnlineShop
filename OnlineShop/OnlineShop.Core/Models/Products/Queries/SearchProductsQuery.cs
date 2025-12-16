using MediatR;

namespace OnlineShop.Core.Models.Products.Queries
{
    public record SearchProductsQuery(string Query) : IRequest<List<ProductDto>>;

}
