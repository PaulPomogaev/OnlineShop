using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class CartJsonRepository
    {
        private static string _filepath = "Data/carts.json";
        private static int _nextItemId = 1;
               
        private static List<Cart> GetAllCarts()
        {
            if (!File.Exists(_filepath))
            {
                return new List<Cart>();
            }
            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Cart>>(json) ?? new List<Cart>();
        }

        public static Cart GetCart(string userId = "guest")
        {
            var carts = GetAllCarts();
            var cart = carts.FirstOrDefault(c => c.UserId == userId);

            if(cart == null)
            {
                cart = new Cart { Id = carts.Any() ? carts.Max(c => c.Id) + 1 : 1, UserId = userId };
                carts.Add(cart);
                SaveAllCarts(carts);
            }

            return cart;
        }

        public static void SaveAllCarts(List<Cart> carts)
        {
            var json = JsonSerializer.Serialize(carts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
        }

        public static void SaveCart (Cart cart)
        {
            var carts = GetAllCarts();
            var existingCart = carts.FirstOrDefault(c => c.UserId == cart.UserId);

            if(existingCart != null)
            {
                existingCart.Items = cart.Items;
            }
            else
            {
                cart.Id = carts.Any() ? carts.Max(c => c.Id) + 1 : 1;
                carts.Add(cart);
            }
              
            SaveAllCarts(carts);
        }

        public static void AddToCart(int productId, int quantity = 1, string userId = "guest")
        {
            var cart = GetCart(userId);
            var product = ProductJsonRepository.ReturnById(productId);

            if(product == null)
            {
                return;
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.Product.Id == productId);

            if(existingItem != null)
            {
                existingItem.Quantity += quantity;

            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = _nextItemId++,
                    Product = product,
                    Quantity = quantity
                });
            }
            SaveCart(cart);
        }

        public static void RemoveFromCart(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);

            var itemToRemove = cart.Items.FirstOrDefault(item => item.Id == itemId);

            if(itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                SaveCart(cart);
            }
        }

        public static void ClearCart(string userId = "guest")
        {
            var cart = GetCart(userId);
            cart.Items.Clear();
            SaveCart(cart);
        }
    }
}
