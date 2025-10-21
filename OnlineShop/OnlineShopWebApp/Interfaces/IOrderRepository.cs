using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Create(Cart cart);
        Order Create(Cart cart, OrderInputModel input);
        void Edit(Order order);
    }
}
