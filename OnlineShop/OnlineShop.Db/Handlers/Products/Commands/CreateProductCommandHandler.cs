using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;

        public CreateProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var existingProducts = await _productQueryRepository.SearchEngineAsync(command.Name);
            if (existingProducts.Any(p => p.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase)))
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
            return product.Id;
        }
    }
}
