using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Models;

namespace Somali_Market_Hub.Data
{
    public class SMHDbContext : DbContext
    {
        public SMHDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        protected SMHDbContext()
        {
        }
    }
}
