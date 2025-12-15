using OnlineShop.Core.Interfaces.Cqrs;

namespace OnlineShop.Core.Models.Products.Queries
{
 public record GetProductByIdQuery(int id) : IQuery<ProductDto?>;
}
