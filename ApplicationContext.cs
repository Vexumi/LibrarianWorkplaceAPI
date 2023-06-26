using Microsoft.EntityFrameworkCore;

namespace LibrarianWorkplaceAPI
{
    public class ApplicationContext : DbContext
    {
        public DbSet<BookModel> Books { get; set; } = null!;
        public DbSet<ReaderModel> Readers { get; set; } = null!;

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookModel>().HasIndex(b => b.Id).IsUnique();
            modelBuilder.Entity<ReaderModel>().HasIndex(r => r.Id).IsUnique();
        }
    }
}
