using Microsoft.Extensions.Caching.Memory;
using MediatR;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;


namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IMemoryCache _cache;

        public CreateProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository, IMemoryCache cache)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var command = request;
            var existingProducts = await _productQueryRepository.SearchEngineAsync(command.Name);
            if (existingProducts.Any())
            {
                throw new InvalidOperationException("Продукт с таким названием уже существует");
            }

            var product = new Product
            {
                Name = command.Name,
                Cost = command.Cost,
                Description = command.Description,
                PhotoPath = command.PhotoPath,
                ImagePaths = command.ImagePaths
            };

            await _productCommandRepository.AddAsync(product);
            _cache.Remove("all_products");
            return product.Id;
        }
    }
}
