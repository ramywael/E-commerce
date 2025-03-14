using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;

namespace E_Commerce510.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
