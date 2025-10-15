using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        void Add(Order order);
        Order? GetById(int id);
        Order Create(Cart cart);
        Order Create(Cart cart, OrderInputModel input);
        void Edit(Order order);
    }
}
