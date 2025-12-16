using MediatR;

namespace OnlineShop.Core.Models.Products.Queries
{
    public record GetAllProductsQuery : IRequest<List<ProductDto>>;

}
