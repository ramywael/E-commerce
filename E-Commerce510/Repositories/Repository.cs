using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using E_Commerce510.Repositories.Repositories;

namespace E_Commerce510.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext) {
          this._dbContext = dbContext;
          dbSet= _dbContext.Set<T>();
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }

        public void CreateAll(List<T> entities)
        {
            dbSet.AddRange(entities);
        }



        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }


        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool isTrack = true)
        {
            IQueryable<T> entities = dbSet;
            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);

                }
            }

            if (isTrack)
            {
                entities = entities.AsNoTracking();
            }

            return entities;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool isTrack = true)
        {
            return Get(filter, includes, isTrack).FirstOrDefault();
        }

    }
}
