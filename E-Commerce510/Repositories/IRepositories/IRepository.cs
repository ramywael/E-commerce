using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_Commerce510.Repositories.Repositories
{
    public interface IRepository<T> where T : class
    {
        public void Create(T entity);

        public void Edit(T entity);


        public void Delete(T entity);

        public void DeleteAll(List<T> entities);
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool isTrack = true);
        public void Commit();

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool isTrack = true);
    }
}
