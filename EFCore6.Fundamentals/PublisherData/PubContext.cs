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

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Rhoda", LastName = "Lerman" });
            var authorList = new Author[]{
                new Author { Id = 2, FirstName = "Ruth", LastName = "Ozeki" },
                new Author { Id = 3, FirstName = "Sofia", LastName = "Segovia" },
                new Author { Id = 4, FirstName = "Ursula K.", LastName = "LeGuin" },
                new Author { Id = 5, FirstName = "Hugh", LastName = "Howey" },
                new Author { Id = 6, FirstName = "Isabelle", LastName = "Allende" }
            };               
            modelBuilder.Entity<Author>().HasData(authorList);

            var someBooks = new Book[]{
                new Book {BookId = 1, AuthorFK=1, Title = "In God's Ear",
                    PublishDate= new DateTime(1989,3,1) },
                new Book {BookId = 2, AuthorFK=2, Title = "A Tale For the Time Being",
                PublishDate = new DateTime(2013,12,31) },
                new Book {BookId = 3, AuthorFK=3, Title = "The Left Hand of Darkness",
                PublishDate=(DateTime)new DateTime(1969,3,1)} };
            modelBuilder.Entity<Book>().HasData(someBooks);

            var someArtists = new Artist[]{
                new Artist { Id = 1, FirstName = "Pablo", LastName="Picasso"},
                new Artist { Id = 2, FirstName = "Dee", LastName="Bell"},
                new Artist { Id = 3, FirstName ="Katharine", LastName="Kuharic"} };
            modelBuilder.Entity<Artist>().HasData(someArtists);

            var someCovers = new Cover[]{
                new Cover { Id = 1, DesignIdeas="How about a left hand in the dark?", DigitalOnly=false, BookId=3},
                new Cover { Id = 2, DesignIdeas= "Should we put a clock?", DigitalOnly=true, BookId=2},
                new Cover { Id = 3, DesignIdeas="A big ear in the clouds?", DigitalOnly = false, BookId=1}};
            modelBuilder.Entity<Cover>().HasData(someCovers);

            //example of mapping skip navigation with payload
            //modelBuilder.Entity<Artist>()
            //    .HasMany(a => a.Covers)
            //    .WithMany(c => c.Artists)
            //    .UsingEntity<CoverAssignment>(
            //       join => join
            //        .HasOne<Cover>()
            //        .WithMany()
            //        .HasForeignKey(ca => ca.CoverId),
            //       join => join
            //        .HasOne<Artist>()
            //        .WithMany()
            //        .HasForeignKey(ca => ca.ArtistId));
            //modelBuilder.Entity<CoverAssignment>()
            //            .Property(ca => ca.DateCreated).HasDefaultValueSql("GetDate()");
            //modelBuilder.Entity<CoverAssignment>()
            //             .Property(ca => ca.CoverId).HasColumnName("CoversCoverId");
            //modelBuilder.Entity<CoverAssignment>()
            //             .Property(ca => ca.ArtistId).HasColumnName("ArtistsArtistId");


            //example of mapping an unconventional FK
            //since I have the author prop in books, I am
            //using it in WithOne:
            //modelBuilder.Entity<Author>()
            //   .HasMany(a => a.Books)
            //   .WithOne(b => b.Author)
            //   .HasForeignKey(b=>b.AuthorId).IsRequired(false);


            //example of a more advanced mapping to specify
            //a one to many between author and book when 
            //there are no navigation properties:
            //modelBuilder.Entity<Author>()
            //    .HasMany<Book>()
            //    .WithOne();

        }
    }
}