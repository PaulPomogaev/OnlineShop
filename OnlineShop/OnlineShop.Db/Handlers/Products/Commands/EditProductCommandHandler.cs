using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class EditProductCommandHandler : ICommandHandler<EditProductCommand, bool>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;

        public EditProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
        }

        public async Task<bool> Handle(EditProductCommand command, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _productQueryRepository.GetByIdAsync(command.Id);

            if(existingProduct == null)
            {
                return false;
            }

            var existingProducts = await _productQueryRepository.SearchEngineAsync(command.Name);
            if (existingProducts.Any(p => p.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase) && p.Id != command.Id))
            {
                throw new InvalidOperationException("Продукт с таким названием уже существует");
            }

            existingProduct.Name = command.Name;
            existingProduct.Cost = command.Cost;
            existingProduct.Description = command.Description;
            existingProduct.PhotoPath = command.PhotoPath;
            existingProduct.ImagePaths = command.ImagePaths;

            await _productCommandRepository.EditAsync(existingProduct);
            return true;
        }
    }
}
