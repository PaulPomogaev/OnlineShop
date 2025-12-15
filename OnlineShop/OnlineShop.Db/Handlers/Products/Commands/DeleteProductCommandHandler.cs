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
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;

        public DeleteProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _productQueryRepository.GetByIdAsync(command.Id);

            if(existingProduct == null)
            {
                return false;
            }

            await _productCommandRepository.DeleteAsync(command.Id);
            return true;
        }
    }
}
