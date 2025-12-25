using OnlineShop.Core.Interfaces.Cqrs;

namespace OnlineShop.Core.Models.Products.Commands
{
    public record EditProductCommand(int Id, string Name, decimal Cost, string? Description = null, string? PhotoPath = null, List<string>? ImagePaths = null) : ICommand<bool>;
    
}
