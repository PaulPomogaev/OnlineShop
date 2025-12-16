using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache;

        public DeleteProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _productQueryRepository.GetByIdAsync(command.Id);

            if(existingProduct == null)
            {
                return false;
            }

            await _productCommandRepository.DeleteAsync(command.Id);

            _cache.Remove("all_products");
            _cache.Remove($"product_{command.Id}");

            return true;
        }
    }
}
