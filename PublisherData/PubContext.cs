using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublisherDomain;

namespace PublisherData
{
    public class PubContext : DbContext
    {
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<AuthorByArtist> AuthorsByArtist { get; set; }

        private StreamWriter _writer = new StreamWriter("EFCoreLog.txt", append: true);

        public PubContext() { }
        public PubContext(DbContextOptions<PubContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { 
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog=PubDatabase")
                .LogTo(Console.WriteLine,
                new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information) //log only information log levels, can also use for warn and debug, removing this will show all
                .EnableSensitiveDataLogging();//ability to control if senstive info can be shown in the logs or not
            }
        }

        //using a stream writer to log in a file
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubDatabase"
            ).LogTo(_writer.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information)
             .EnableSensitiveDataLogging();
        }
        public override void Dispose()
        {
            _writer.Dispose();
            base.Dispose();
        }*/

        //seeding initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AuthorByArtist>().HasNoKey()
            .ToView(nameof(AuthorsByArtist));

            modelBuilder.Entity<Author>().HasData(
                         new Author { AuthorId = 1, FirstName = "Rhoda", LastName = "Lerman" });

            var authorList = new Author[]{
                new Author {AuthorId = 2, FirstName = "Ruth", LastName = "Ozeki" },
                new Author {AuthorId = 3, FirstName = "Sofia", LastName = "Segovia" },
                new Author {AuthorId = 4, FirstName = "Ursula K.", LastName = "LeGuin" },
                new Author {AuthorId = 5, FirstName = "Hugh", LastName = "Howey" },
                new Author {AuthorId = 6, FirstName = "Isabelle", LastName = "Allende" }
            };
            modelBuilder.Entity<Author>().HasData(authorList);

            var someBooks = new Book[]{
               new Book {BookId = 1, AuthorId=1, Title = "In God's Ear",
                   PublishDate= new DateOnly(1989,3,1) },
               new Book {BookId = 2, AuthorId=2, Title = "A Tale For the Time Being",
                   PublishDate = new DateOnly(2013,12,31) },
               new Book {BookId = 3, AuthorId=3, Title = "The Left Hand of Darkness",
                   PublishDate=new DateOnly(1969,3,1)},
            };

            modelBuilder.Entity<Book>().HasData(someBooks);

            var someArtists = new Artist[]{
            new Artist {ArtistId = 1, FirstName = "Pablo", LastName="Picasso"},
            new Artist {ArtistId = 2, FirstName = "Dee", LastName="Bell"},
            new Artist {ArtistId = 3, FirstName ="Katharine", LastName="Kuharic"} };
            modelBuilder.Entity<Artist>().HasData(someArtists);

            var someCovers = new Cover[]{
            new Cover {CoverId = 1, BookId=3,
                DesignIdeas="How about a left hand in the dark?", DigitalOnly=false},
            new Cover {CoverId = 2, BookId=2,
                DesignIdeas= "Should we put a clock?", DigitalOnly=true},
            new Cover {CoverId = 3, BookId=1,
                DesignIdeas="A big ear in the clouds?", DigitalOnly = false}};
            modelBuilder.Entity<Cover>().HasData(someCovers);


            //way to make entity framework aware of the realtionship between two entities
            /*modelBuilder.Entity<Author>()
                .HasMany<Book>()
                .WithOne();*/

            /*modelBuilder.Entity<Author>()
                .HasMany(a=>a.Books)
                .WithOne(b=>b.Author)
                .HasForeignKey(b=>b.AuthorId)
                .IsRequired(false);*/

            /*example of mapping a skip navigation with payload
            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Covers)
                .WithMany(c => c.Artists)
                .UsingEntity<CoverAssignment>(
                    b =>
                    {
                        b.Property(ca => ca.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        b.ToTable("ArtistCover"); //last 3 mappings are because of a pre-existing table
                        b.Property(ca => ca.CoverId).HasColumnName("CoversCoverId");
                        b.Property(ca => ca.ArtistId).HasColumnName("ArtistsArtistId");
                    });*/

            /*modelBuilder.Entity<Author>()
            .InsertUsingStoredProcedure("AuthorInsert", spbuilder =>
               spbuilder.HasParameter(a => a.FirstName)
                        .HasParameter(a => a.LastName)
                        .HasResultColumn(a => a.AuthorId));*/
        }
    }
}