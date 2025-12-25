using OnlineShop.Core.Interfaces.Cqrs;

namespace OnlineShop.Core.Models.Products.Commands
{
    public record DeleteProductCommand(int Id) : ICommand<bool>;

}
