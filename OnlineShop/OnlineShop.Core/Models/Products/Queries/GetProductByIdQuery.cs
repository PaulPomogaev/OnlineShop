using MediatR;

namespace OnlineShop.Core.Models.Products.Queries
{
 public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
}
