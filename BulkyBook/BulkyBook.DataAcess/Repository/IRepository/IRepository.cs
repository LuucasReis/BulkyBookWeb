using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
        public T GetById(int id, string? includeProperties = null);
        public void Add(T entity);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties=null);
        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entitys);

    }
}
