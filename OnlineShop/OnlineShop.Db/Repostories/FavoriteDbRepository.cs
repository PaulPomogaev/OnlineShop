using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace OnlineShop.Db.Repostories
{
    public class FavoriteDbRepository : IFavoriteRepository
    {
        private readonly DatabaseContext _context;

        public FavoriteDbRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Favorite? Get(string userId = "guest")
        {

            return _context.Favorites.Include(f => f.Items).ThenInclude(cp => cp.Product).FirstOrDefault(c => c.UserId == userId);
        }
            

        public void Add(int productId, string userId = "guest")
        {
            var favorite = Get(userId);

            if (favorite == null)
            {
                favorite = new Favorite { UserId = userId, Items = new List<FavoriteItem>() };
                _context.Favorites.Add(favorite);
            }

            if (!favorite.Items.Any(i => i.ProductId == productId))
            {
                favorite.Items.Add(new FavoriteItem { ProductId = productId });
                _context.SaveChanges();
            }
        }

        public void Clear(string userId = "guest")
        {
            var favorite = Get(userId);
            if (favorite != null)
            {
                favorite.Items.Clear();
                _context.SaveChanges();
            }
        }

        public void Remove(int productId, string userId = "guest")
        {
            var favorite = Get(userId);

            if (favorite != null)
            {
                var itemToRemove = favorite.Items.FirstOrDefault(i => i.ProductId == productId);
                if(itemToRemove != null)
                {
                    favorite.Items.Remove(itemToRemove);
                    _context.SaveChanges();
                }
            }
        }
    }
}
