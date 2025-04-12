using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void DeleteRange(IEnumerable<Cart> carts)
        {
            dbContext.Carts.RemoveRange(carts);
        }
    }
}
