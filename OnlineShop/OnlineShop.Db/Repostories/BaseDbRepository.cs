using Microsoft.EntityFrameworkCore;
using OnlineShop.Core.Interfaces;
using System.Text.Json;
using System.Text;

namespace OnlineShop.Db.Repostories
{
    public abstract class BaseDbRepository<T> : IRepository<T> where T : class, IBaseId
    {
        protected readonly DatabaseContext _context;
        
        public BaseDbRepository(DatabaseContext context)
        {
            _context = context;
        }

        public virtual List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        
        public virtual void Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
           var item = _context.Set<T>().FirstOrDefault(entity => entity.Id == id);

            if (item != null)
            {
                _context.Set<T>().Remove(item);
                _context.SaveChanges();
            }
        }

        public virtual T? GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(entity => entity.Id == id);
        }
    }
}
