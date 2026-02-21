using LibraryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Repositories.EF
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
            .OwnsOne(b => b.Isbn, isbn =>
            {
                isbn.Property(i => i.Value)
                    .HasColumnName("Isbn");

                isbn.HasIndex(i => i.Value)
                    .IsUnique()
                    .HasFilter(null); // для SQL Server можно указать фильтр
            });

            modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);
        }
    }
}
