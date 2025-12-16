using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using OnlineShop.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace OnlineShop.Db.Repostories
{
    public class OrderDbRepository : BaseDbRepository<Order>, IOrderRepository
    {
        private readonly DatabaseContext _context;

        public OrderDbRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public void SaveOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public Order Create(Cart cart)
        {
            return Create(cart, null);
        }

        public Order Create(Cart cart, OrderInputModel? input = null)
        {
            if (input == null)
            {
                input = new OrderInputModel
                {
                    CustomerName = string.Empty,
                    Address = string.Empty,
                    Phone = string.Empty,
                    DeliveryDate = null,
                    Comment = string.Empty
                };
            }

            var order = new Order
            {
                UserId = cart.UserId,
                Customer = new CustomerInfo
                {
                    Name = input.CustomerName,
                    Address = input.Address,
                    Phone = input.Phone
                },
                DeliveryDate = input.DeliveryDate,
                Comment = input.Comment,
                CreatedDate = DateTime.Now,
                Status = OrderStatus.Created,
                Items = new List<OrderItem>()
            };
                        
            foreach (var cartItem in cart.Items)
            {
                if (cartItem?.Product == null) continue;
              
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Cost, 
                    Product = cartItem.Product
                };
                order.Items.Add(orderItem);
            }
            
            return order;
        }

        public void Edit(Order updateOrder)
        {
            var existingOrder = _context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == updateOrder.Id);

            if (existingOrder != null)
            {
                existingOrder.Status = updateOrder.Status;
                _context.SaveChanges();
            }
        }

        public override Order? GetById(int id)
        {
            
            var order = _context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == id);
                        
            return order;
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).Include(o => o.Items).ThenInclude(oi => oi.Product).OrderBy(o => o.CreatedDate).ToList();
        }
    }
}

