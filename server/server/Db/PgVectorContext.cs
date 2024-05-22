using client.Model;
using Microsoft.EntityFrameworkCore;

namespace client.Db
{
    public class PgVectorContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<DocumentChunk> DocumentChunks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("postgres", o => o.UseVector());
        }

    }

}
