using MediatR;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Handlers.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly ICacheService _cache;

        public DeleteProductCommandHandler(IProductCommandRepository productCommandRepository, IProductQueryRepository productQueryRepository, ICacheService cache)
        {
            _productCommandRepository = productCommandRepository;
            _productQueryRepository = productQueryRepository;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var command = request;
            var existingProduct = await _productQueryRepository.GetByIdAsync(command.Id);

            if(existingProduct == null)
            {
                return false;
            }

            await _productCommandRepository.DeleteAsync(command.Id);

            await _cache.RemoveAsync(Constants.AllProducts);
            await _cache.RemoveAsync($"product_{command.Id}");

            return true;
        }
    }
}
