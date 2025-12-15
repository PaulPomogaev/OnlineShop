using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using OnlineShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Db.Repostories
{
    public class ProductQueryRepository : IProductQueryRepository
    {
        private readonly DatabaseContext _context;

        public ProductQueryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
               
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> SearchEngineAsync(string query)
        {
           if(string.IsNullOrWhiteSpace(query))
            {
                return await GetAllAsync();
            }

            return await _context.Products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }
    }
}
