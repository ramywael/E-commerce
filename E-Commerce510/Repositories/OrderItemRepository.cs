using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
