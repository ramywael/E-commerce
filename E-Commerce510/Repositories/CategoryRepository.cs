using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;

namespace E_Commerce510.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
