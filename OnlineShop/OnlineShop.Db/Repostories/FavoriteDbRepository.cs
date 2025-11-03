using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using Microsoft.EntityFrameworkCore;

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

            return _context.Favorites.Include(f => f.Products).FirstOrDefault(f => f.UserId == userId);
        }
            

        public void Add(int productId, string userId = "guest")
        {
            var favorite = Get(userId);

            if (favorite == null)
            {
                favorite = new Favorite { UserId = userId};
                _context.Favorites.Add(favorite);
            }

            var product = _context.Products.Find(productId);
            if (product != null && !favorite.Products.Any(p => p.Id == productId))
            {
                favorite.Products.Add(product);
                _context.SaveChanges();
            }
        }

        public void Clear(string userId = "guest")
        {
            var favorite = Get(userId);
            if (favorite != null)
            {
                favorite.Products.Clear();
                _context.SaveChanges();
            }
        }

        public void Remove(int productId, string userId = "guest")
        {
            var favorite = Get(userId);

            if (favorite != null)
            {
                var productToRemove = favorite.Products.FirstOrDefault(p => p.Id == productId);
                if (productToRemove != null)
                {
                    favorite.Products.Remove(productToRemove);
                    _context.SaveChanges();
                }
            }
        }
    }
}
