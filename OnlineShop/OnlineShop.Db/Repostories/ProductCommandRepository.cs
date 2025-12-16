using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Db.Repostories
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly DatabaseContext _context;

        public ProductCommandRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product != null)
            {
                _context.Products.Remove(product);
               await _context.SaveChangesAsync();
            }
        }

        public async Task EditAsync(Product updateProduct)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == updateProduct.Id);

            if(existingProduct != null)
            {
                existingProduct.Name = updateProduct.Name;
                existingProduct.Cost = updateProduct.Cost;
                existingProduct.Description = updateProduct.Description;
                existingProduct.PhotoPath = updateProduct.PhotoPath;
                existingProduct.ImagePaths = updateProduct.ImagePaths;

                await _context.SaveChangesAsync();
            }
        }
    }
}
