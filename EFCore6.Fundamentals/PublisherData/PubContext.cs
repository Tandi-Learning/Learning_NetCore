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
                new Book { BookId = 2, AuthorFK = 1, Title = "The Shinning"},
                new Book { BookId = 3, AuthorFK = 1, Title = "Salem's Lot"},
                new Book { BookId = 4, AuthorFK = 2, Title = "Evil Under The Sun"},
                new Book { BookId = 5, AuthorFK = 2, Title = "Murder on the Orient Express "},
                new Book { BookId = 6, AuthorFK = 2, Title = "Death On The Nile"},
                new Book { BookId = 7, AuthorFK = 2, Title = "The ABC Murders"},
                new Book { BookId = 8, AuthorFK = 3, Title = "2001: A Space Odyssey"},
                new Book { BookId = 9, AuthorFK = 3, Title = "2010: Odyssey Two"}
            };
            modelBuilder.Entity<Book>().HasData(books);

            List<Artist> artists = new List<Artist>
            {
                new Artist {Id = 1, FirstName = "Pablo", LastName = "Picasco"},
                new Artist {Id = 2, FirstName = "Beaux", LastName = "Arts"},
                new Artist {Id = 3, FirstName = "Tom", LastName = "Bonson"},
            };
            modelBuilder.Entity<Artist>().HasData(artists);

            List<Cover> covers = new List<Cover>
            {
                new Cover {Id = 1, DesignIdeas = "The Song of the Dead", DigitalOnly = false},
                new Cover {Id = 2, DesignIdeas = "Vallauris Exhibition", DigitalOnly = false},
                new Cover {Id = 3, DesignIdeas = "Diary Of An Anorexic", DigitalOnly= false},
                new Cover {Id = 4, DesignIdeas = "Stranger Is The Night", DigitalOnly= false},
                new Cover {Id = 5, DesignIdeas = "The Phantom Fossil", DigitalOnly= false},
                new Cover {Id = 6, DesignIdeas = "Welcome To The Jungle", DigitalOnly= false},
            };
            modelBuilder.Entity<Cover>().HasData(covers);

        }
    }
}