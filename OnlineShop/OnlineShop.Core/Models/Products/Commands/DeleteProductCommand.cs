using MediatR;

namespace OnlineShop.Core.Models.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;

}
