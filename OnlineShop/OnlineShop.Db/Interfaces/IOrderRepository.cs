using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;

namespace OnlineShop.Db.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void SaveOrder(Order order);
        Order Create(Cart cart);
        Order Create(Cart cart, OrderInputModel input);
        void Edit(Order order);
        List<Order> GetUserOrders(string userId);
    }
}
