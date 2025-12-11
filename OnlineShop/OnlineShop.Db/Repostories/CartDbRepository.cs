using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Db.Repostories
{
    public class CartDbRepository : ICartRepository
    {
        private readonly DatabaseContext _context;
       
        public CartDbRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Cart GetCart(string userId = "guest")
        {
            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }


        public void AddToCart(int productId, int quantity = 1, string userId = "guest")
        {
            if (!_context.Products.Any(p => p.Id == productId))
            {
                return;
            }

            var cart = GetCart(userId);
            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            _context.SaveChanges();
        }

        public void RemoveFromCart(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cart.Items.Remove(item);
                }
                _context.SaveChanges();
            }
        }

        public void ClearCart(string userId = "guest")
        {
            var cart = GetCart(userId);
            cart.Items.Clear();
            _context.SaveChanges();
        }

        public void IncreaseItemQuantity(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                item.Quantity++;
                _context.SaveChanges();
            }
        }

        public void DecreaseItemQuantity(int itemId, string userId = "guest")
        {
            var cart = GetCart(userId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cart.Items.Remove(item);
                }
                _context.SaveChanges();
            }
        }

        public int GetCartItemCount(string userId = "guest")
        {
            var cart = GetCart(userId);
            return cart.Items.Sum(i => i.Quantity);
        }
    }
}
