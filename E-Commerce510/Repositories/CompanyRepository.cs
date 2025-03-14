using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;

namespace E_Commerce510.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
