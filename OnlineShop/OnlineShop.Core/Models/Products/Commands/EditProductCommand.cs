using OnlineShop.Core.Interfaces.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models.Products.Commands
{
    public record EditProductCommand(int Id, string Name, decimal Cost, string? Description = null, string? PhotoPath = null, List<string>? ImagePaths = null) : ICommand<bool>;
    
}
