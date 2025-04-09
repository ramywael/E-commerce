using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
