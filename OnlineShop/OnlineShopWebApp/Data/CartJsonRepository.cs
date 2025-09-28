using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class CartJsonRepository : ICartRepository
    {
        private readonly string _filepath = "Data/carts.json";
        private int _nextItemId;
        private readonly IProductRepository _productRepository;

        public CartJsonRepository(IProductRepository productJsonRepository)
        {
            _productRepository = productJsonRepository;
            InitializeNextItemId();
        }

        private void InitializeNextItemId()
        {
            var carts = GetAll();
            var allItemsIds = carts.SelectMany(c => c.Items.Select(i => i.Id)).ToList();
            _nextItemId = allItemsIds.Any() ? allItemsIds.Max() + 1 : 1;
        }

        private List<Cart> GetAll()
        {
            if (!File.Exists(_filepath))
            {
                return new List<Cart>();
            }
            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Cart>>(json) ?? new List<Cart>();
        }

        public Cart GetCart(string userId = "guest")
        {
            var carts = GetAll();
            var cart = carts.FirstOrDefault(c => c.UserId == userId);

            if(cart == null)
            {
                cart = new Cart { Id = carts.Any() ? carts.Max(c => c.Id) + 1 : 1, UserId = userId };
                carts.Add(cart);
                SaveAllCarts(carts);
            }

            return cart;
        }

        public void SaveAllCarts(List<Cart> carts)
        {
            var json = JsonSerializer.Serialize(carts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
        }

        public void SaveCart (Cart cart)
        {
            var carts = GetAll();
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

        public void AddToCart(int productId, int quantity = 1, string userId = "guest")
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return;
            }

            var cart = GetCart(userId);
            
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

        public void RemoveFromCart(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);

            var itemToRemove = cart.Items.FirstOrDefault(item => item.Id == itemId);

            if (itemToRemove != null)
            {
                if (itemToRemove.Quantity > 1)
                {
                    itemToRemove.Quantity--;
                }
                else
                {
                    cart.Items.Remove(itemToRemove);
                }
                SaveCart(cart);
            }
        }

        public void ClearCart(string userId = "guest")
        {
            var cart = GetCart(userId);
            cart.Items.Clear();
            SaveCart(cart);
        }

        public void IncreaseItemQuantity(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return;
            }

            item.Quantity++;

            SaveCart(cart);
        }

        public void DecreaseItemQuantity(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return;
            }
            if(item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                cart.Items.Remove(item);
            }
            SaveCart(cart);
        }

        public int GetCartItemCount(string userId = "guest")
        {
            var cart = GetCart(userId);
            return cart.Items.Sum(i => i.Quantity);
        }
    }
}
