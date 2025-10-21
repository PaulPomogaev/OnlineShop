namespace OnlineShopWebApp.Interfaces
{
    public interface IRepository<T> where T: class, IBaseId
    {
        List<T> GetAll();
        T? GetById(int id);
        void Add(T item);
        void Delete(int id);
    }
}
