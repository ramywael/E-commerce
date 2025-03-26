using E_Commerce510.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Commerce510.Models.ViewModel;

namespace E_Commerce510.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Company> Companies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }


        //Legacy Code (Without DI)

        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=E-Commerce510;Integrated Security=True;TrustServerCertificate=True");
        }
        public DbSet<E_Commerce510.Models.ViewModel.RegisterVm> RegisterVm { get; set; } = default!;
        public DbSet<E_Commerce510.Models.ViewModel.LoginVm> LoginVm { get; set; } = default!;

    }
}
