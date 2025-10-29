using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using OnlineShop.Db.Repostories;

namespace OnlineShop.Db
{
    public class ProductDbRepository : BaseDbRepository<Product>, IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductDbRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        
                
        public void Edit(Product updateProduct)
        {
           var existingProduct = _context.Products.FirstOrDefault(p => p.Id == updateProduct.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = updateProduct.Name;
                existingProduct.Cost = updateProduct.Cost;
                existingProduct.Description = updateProduct.Description;
                _context.SaveChanges();
            }
        }

        
        public List<Product> SearchEngine(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                return GetAll();
            }

            var allProducts = GetAll();
            return _context.Products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
                
    }
}