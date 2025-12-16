using MediatR;

namespace OnlineShop.Core.Models.Products.Commands
{
    public record EditProductCommand(int Id, string Name, decimal Cost, string? Description = null, string? PhotoPath = null, List<string>? ImagePaths = null) : IRequest<bool>;
    
}
