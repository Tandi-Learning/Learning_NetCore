using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublisherData
{
    public class PubContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cover> Covers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Data Source = (localdb)\\ProjectModels; Initial Catalog = PubDatabase"
            ).LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(c => c.AuthorFK);

            List<Author> authors = new List<Author>
            {
                new Author {Id = 1, FirstName = "Stephen", LastName = "King"},
                new Author {Id = 2, FirstName = "Agatha", LastName = "Christie"},
                new Author {Id = 3, FirstName = "Arthur", LastName = "Clarke"},
            };
            modelBuilder.Entity<Author>().HasData(authors);

            List<Book> books = new List<Book>
            {
                new Book { BookId = 1, AuthorFK = 1, Title = "Pet's Semetary"},
                new Book { BookId = 2, AuthorFK = 2, Title = "Evil Under The Sun"},
                new Book { BookId = 3, AuthorFK = 3, Title = "Space Odyssey 2001"}
            };
            modelBuilder.Entity<Book>().HasData(books);
        }
    }
}