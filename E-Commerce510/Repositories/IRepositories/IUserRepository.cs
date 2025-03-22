using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;
using System.Linq.Expressions;

namespace E_Commerce510.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUsersInRole(string roleName, Expression<Func<ApplicationUser, bool>>? filter = null);
    }
}
