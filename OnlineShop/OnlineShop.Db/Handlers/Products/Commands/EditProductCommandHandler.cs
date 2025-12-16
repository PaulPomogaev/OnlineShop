using Microsoft.Extensions.Caching.Memory;
using MediatR;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, bool>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache;

        public EditProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<bool> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var command = request;
            var existingProduct = await _productQueryRepository.GetByIdAsync(command.Id);

            if(existingProduct == null)
            {
                return false;
            }

            var existingProducts = await _productQueryRepository.SearchEngineAsync(command.Name);
            if (existingProducts.Any(p => p.Id != command.Id))
            {
                throw new InvalidOperationException("Продукт с таким названием уже существует");
            }

            existingProduct.Name = command.Name;
            existingProduct.Cost = command.Cost;
            existingProduct.Description = command.Description;
            existingProduct.PhotoPath = command.PhotoPath;
            existingProduct.ImagePaths = command.ImagePaths;

            await _productCommandRepository.EditAsync(existingProduct);

            _cache.Remove("all_products");
            _cache.Remove($"product_{command.Id}");

            return true;
        }
    }
}
