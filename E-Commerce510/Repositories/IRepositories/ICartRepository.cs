using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;

namespace E_Commerce510.Repositories.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        public void DeleteRange(IEnumerable<Cart> carts);
    }
}
