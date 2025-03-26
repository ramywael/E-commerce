using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace E_Commerce510.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<ApplicationUser> GetUsersInRole(string roleName, Expression<Func<ApplicationUser, bool>>? filter = null)
        {
            var usersInRole = from users in _dbContext.Users
                              join userRole in _dbContext.UserRoles on users.Id equals userRole.UserId
                              join role in _dbContext.Roles on userRole.RoleId equals role.Id
                              where role.Name == roleName
                              select users;

            if (filter != null)
            {
                usersInRole = usersInRole.Where(filter);
            }


            return usersInRole;
        }
    }
}
