using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Text;
using OnlineShop.Core.Models;


namespace OnlineShop.Db.Repostories
{
    public class OrderDbRepository : BaseDbRepository<Order>, IOrderRepository
    {
        private readonly DatabaseContext _context;

        public OrderDbRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public override void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public Order Create(Cart cart)
        {
            var input = new OrderInputModel
            {
                CustomerName = string.Empty,
                Address = string.Empty,
                Phone = string.Empty,
                DeliveryDate = null,
                Comment = string.Empty
            };
            return Create(cart, input);
        }

        public Order Create(Cart cart, OrderInputModel input)
        {
            var orderItems = cart.Items.Select(item => new OrderItem
            {
                Product = item.Product,
                Quantity = item.Quantity
            }).ToList();

            return new Order
            {
                UserId = cart.UserId,
                Items = orderItems,
                Customer = new CustomerInfo
                {
                    Name = input.CustomerName,
                    Address = input.Address,
                    Phone = input.Phone
                },
                DeliveryDate = input.DeliveryDate,
                Comment = input.Comment,
                CreatedDate = DateTime.Now
            };
        }

        public void Edit(Order updateOrder)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == updateOrder.Id);

            if (existingOrder != null)
            {
                existingOrder.Status = updateOrder.Status;
                _context.SaveChanges();
            }
        }
    }
}

