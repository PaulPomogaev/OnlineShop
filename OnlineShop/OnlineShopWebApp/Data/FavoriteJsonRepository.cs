using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class FavoriteJsonRepository : IFavoriteRepository
    {
        private readonly string _filepath = "Data/favorites.json";

        private List<Favorite> GetAll()
        {
            if(!File.Exists(_filepath))
            {
                return new List<Favorite>();
            }

            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Favorite>>(json) ?? new List<Favorite>();
        }

        private void SaveAll(List<Favorite> favorites)
        {
            var json = JsonSerializer.Serialize(favorites, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
        }

        public Favorite Get(string userId = "guest")
        {
            var favorites = GetAll();
            var favorite = favorites.FirstOrDefault(f => f.UserId == userId);

            if (favorite == null)
            {
                favorite = new Favorite { UserId = userId };
                favorites.Add(favorite);
                SaveAll(favorites);
            }

            return favorite;
        }

        public void Add(int productId, string userId = "guest")
        {
            var favorites = GetAll();
            var favorite = favorites.FirstOrDefault(f => f.UserId == userId);

            if (favorite == null)
            {
                favorite = new Favorite { UserId = userId, ProductIds = new List<int>() };
                favorites.Add(favorite);
            }

            if (!favorite.ProductIds.Contains(productId))
            {
                favorite.ProductIds.Add(productId);
                SaveAll(favorites);
            }
        }

        public void Clear(string userId = "guest")
        {
            var favorites = GetAll();
            var favorite = favorites.FirstOrDefault(f => f.UserId == userId);
            if (favorite != null)
            {
                favorite.ProductIds.Clear();
                SaveAll(favorites);
            }
        }

        public void Remove(int productId, string userId = "guest")
        {
            var favorites = GetAll();
            var favorite = favorites.FirstOrDefault(f => f.UserId == userId);

            if (favorite != null && favorite.ProductIds.Contains(productId))
            {
                favorite.ProductIds.Remove(productId);
                SaveAll(favorites);
            }
        }
    }
}
