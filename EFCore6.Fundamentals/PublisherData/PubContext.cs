using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublisherData
{
    public class PubContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Data Source = (localdb)\\ProjectModels; Initial Catalog = PubDatabase"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Author> authors = new List<Author>
            {
                new Author {Id = 1, FirstName = "Stephen", LastName = "King"},
                new Author {Id = 2, FirstName = "Agatha", LastName = "Christie"},
                new Author {Id = 3, FirstName = "Arthur", LastName = "Clarke"},
            };
            modelBuilder.Entity<Author>().HasData(authors);
        }
    }
}