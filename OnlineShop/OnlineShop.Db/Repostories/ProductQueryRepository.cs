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

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<Product> SearchEngine(string query)
        {
           if(string.IsNullOrWhiteSpace(query))
            {
                return GetAll();
            }

            return _context.Products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
