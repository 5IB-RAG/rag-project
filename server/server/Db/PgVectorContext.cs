using server.Model;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace server.Db
{
    public class PgVectorContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DocumentChunk> DocumentChunks { get; set; }

        private readonly string _connectionString;
        
        public PgVectorContext(DbContextOptions<PgVectorContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserChat)
                .WithMany(uc => uc.Messages)
                .HasForeignKey(m => m.ChatId);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<DocumentChunk>()
                .HasOne(dc => dc.Document)
                .WithMany(d => d.Chunks)
                .HasForeignKey(dc => dc.DocumentId);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString, o => o.UseVector());
            }
        }

    }


}
