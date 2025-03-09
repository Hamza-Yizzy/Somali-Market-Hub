using Microsoft.EntityFrameworkCore;

namespace Somali_Market_Hub.Data
{
    public class SMHDbContext : DbContext
    {
        public SMHDbContext(DbContextOptions options) : base(options)
        {
        }

        protected SMHDbContext()
        {
        }
    }
}
