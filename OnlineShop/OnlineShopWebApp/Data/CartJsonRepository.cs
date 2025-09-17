using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class CartJsonRepository
    {
        private static string _filepath = "Data/cart.json";
        private static int _nextCartId = 1;
        private static int _nextItemId = 1;


        static CartJsonRepository()
        {
            InitializeNextIds();
        }

        private static void InitializeNextIds()
        {
            if(!File.Exists(_filepath))
            {
                return;
            }

            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            var cart = JsonSerializer.Deserialize<Cart>(json);

            if(cart != null)
            {
                _nextCartId += cart.Id;
                
                if(cart.Items.Any())
                {
                    _nextItemId = cart.Items.Max(i => i.Id) + 1;
                }
            }
        }

        public static Cart GetCart(string userId = "guest")
        {
            if(!File.Exists(_filepath))
            {
                return new Cart { Id = _nextCartId++, UserId = userId };
            }

            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            var cart = JsonSerializer.Deserialize<Cart>(json);

            if(cart == null)
            {
                return new Cart { Id = _nextCartId++, UserId = userId };
            }

            return cart;
        }

        public static void SaveCart (Cart cart)
        {
            var json = JsonSerializer.Serialize(cart, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
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
