using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Models;

namespace Somali_Market_Hub.Data
{
    public class SMHDbContext : DbContext
    {
        public SMHDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                },
                new Role
                {
                    Id = 2,
                    Name = "Provider",
                },
                new Role
                {
                    Id = 3,
                    Name = "Customer",
                }

                );
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserAccount> Tbl_UserAccounts { get; set; }
        public DbSet<Role> Tbl_Roles { get; set; }
        protected SMHDbContext()
        {
        }
    }
}
