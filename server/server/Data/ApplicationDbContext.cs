using client.Model;
using Microsoft.EntityFrameworkCore;

namespace client.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserAuth> Users { get; set; }
    }
}
