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
                comparison = new Comparison { UserId = userId, Items = new List<ComparisonItem>() };
                _context.Comparisons.Add(comparison);
                
            }

            if(!comparison.Items.Any(i => i.ProductId == productId))
            {
                comparison.Items.Add(new ComparisonItem { ProductId = productId });
                _context.SaveChanges();
            }
        }

        public void Clear(string userId = "guest")
        {
            var comparison = Get(userId);
            
            if(comparison != null)
            {
                comparison.Items.Clear();
                _context.SaveChanges();
            }
        }

        public Comparison? Get(string userId = "guest")
        {
           return _context.Comparisons.Include(f => f.Items).ThenInclude(cp => cp.Product).FirstOrDefault(c => c.UserId == userId);
        }

        public void Remove(int productId, string userId = "guest")
        {
            var comparison = Get(userId);

            if (comparison != null)
            {
                var itemToRemove = comparison.Items.FirstOrDefault(i => i.ProductId == productId);
                if (itemToRemove != null)
                {
                    comparison.Items.Remove(itemToRemove);
                    _context.SaveChanges();
                }
            }
        }

    }
}
