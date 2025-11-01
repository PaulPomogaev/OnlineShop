using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace OnlineShop.Db.Repostories
{
    public class ComparisonDbRepository : IComparisonRepository
    {
        private readonly DatabaseContext _context;

        public ComparisonDbRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(int productId, string userId = "guest")
        {
            var comparison = Get(userId);

            if(comparison == null)
            {
                comparison = new Comparison { UserId = userId};
                _context.Comparisons.Add(comparison);
                
            }

            if (!comparison.Products.Any(p => p.Id == productId))
            {
                var product = _context.Products.Find(productId);
                if (product != null)
                {
                    comparison.Products.Add(product);
                    _context.SaveChanges();
                }
            }
        }

        public void Clear(string userId = "guest")
        {
            var comparison = Get(userId);
            
            if(comparison != null)
            {
                comparison.Products.Clear();
                _context.SaveChanges();
            }
        }

        public Comparison? Get(string userId = "guest")
        {
           return _context.Comparisons.Include(c => c.Products).FirstOrDefault(c => c.UserId == userId);
        }

        public void Remove(int productId, string userId = "guest")
        {
            var comparison = Get(userId);
            if (comparison != null)
            {
                var productToRemove = comparison.Products.FirstOrDefault(p => p.Id == productId);
                if (productToRemove != null)
                {
                    comparison.Products.Remove(productToRemove);
                    _context.SaveChanges();
                }
            }
        }

    }
}
