using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

namespace EFCore8Fundamentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (PubContext context = new PubContext())
            {
                context.Database.EnsureCreated();
            }

            Program pbj = new Program();
            //pbj.AddAuthor();
            pbj.GetAuthors();
            pbj.AddAuthorWithBook();
            pbj.GetAuthorsWithBooks();
        }
        public void AddAuthorWithBook()
        {
            var author = new Author { FirstName = "Julie", LastName = "Lerman" };
            author.Books.Add(new Book
            {
                Title = "Programming Entity Framework",
                PublishDate = new DateOnly(2009, 1, 1)
            });
            author.Books.Add(new Book
            {
                Title = "Programming Entity Framework 2nd Ed",
                PublishDate = new DateOnly(2010, 8, 1)
            });
            using var context = new PubContext();
            context.Authors.Add(author);
            context.SaveChanges();
        }
        public void GetAuthorsWithBooks()
        {
            using var context = new PubContext();
            var authors = context.Authors.Include(a => a.Books).ToList();
            foreach (var author in authors)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName);
                foreach (var book in author.Books)
                {
                    Console.WriteLine(book.Title);
                }
            }
        }

        public void AddAuthor()
        {
            var author = new Author { FirstName = "Josie", LastName = "Newf" };
            using var context = new PubContext();
            context.Authors.Add(author);
            context.SaveChanges();
        }

        public void GetAuthors()
        {
            using var context = new PubContext();
            var authors = context.Authors.ToList();
            foreach (var author in authors)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName);
            }
        }

        public void QueryFiltres()
        {
            PubContext _context = new();
            var authors = _context.Authors.Where(author => author.FirstName == "Josie").ToList();

            //using EF Functions
            var authorsfe = _context.Authors.Where(author=> EF.Functions.Like(author.FirstName, "L%")).ToList();
        }

        public void FindIt()
        {
            PubContext _context = new();
            var authorIdTwo = _context.Authors.Find(2);
        }

        public void AddSomeMoreAuthors()
        {
            PubContext _context = new();
            _context.Authors.Add(new Author { FirstName = "Rhoda", LastName = "Lerman" });
            _context.Authors.Add(new Author { FirstName = "Don", LastName = "Jones" });
            _context.Authors.Add(new Author { FirstName = "Jim", LastName = "Christopher" });
            _context.Authors.Add(new Author { FirstName = "Stephen", LastName = "Haunts" });
            _context.SaveChanges();
        }

        //SkipAndTakeAuthors();
        void SkipAndTakeAuthors()
        {
            PubContext _context = new();
            var groupSize = 2;
            for (int i = 0; i < 5; i++)
            {
                var authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
                Console.WriteLine($"Group {i}:");
                foreach (var author in authors)
                {
                    Console.WriteLine($" {author.FirstName} {author.LastName}");
                }
            }
        }

        //SortAuthors();
        void SortAuthors()
        {
            //if there are multiple orderby methods then linq will ignore but the last one, so we need to use then by
            PubContext _context = new();
            var authorsByLastName = _context.Authors
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName).ToList();
            authorsByLastName.ForEach(a => Console.WriteLine(a.LastName + "," + a.FirstName));

            var authorsDescending = _context.Authors
            .OrderByDescending(a => a.LastName)
            .ThenByDescending(a => a.FirstName).ToList();
            Console.WriteLine("**Descending Last and First**");
            authorsDescending.ForEach(a => Console.WriteLine(a.LastName + "," + a.FirstName));
        }

        void QueryAggregate()
        {
            PubContext _context = new();
            var author = _context.Authors.Where(a => a.LastName == "Lerman").FirstOrDefault();
        }

        //InsertAuthor();
        void InsertAuthor()
        {
            PubContext _context = new();
            var author = new Author { FirstName = "Frank", LastName = "Herbert" };
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        //RetrieveAndUpdateAuthor();
        void RetrieveAndUpdateAuthor()
        {
            PubContext _context = new();
            var author = _context.Authors
                .FirstOrDefault(a => a.FirstName == "Julie" && a.LastName == "Lerman");
            if (author != null)
            {
                author.FirstName = "Julia";
                _context.SaveChanges();
            }
        }

        //RetrieveAndUpdateMultipleAuthors();
        void RetrieveAndUpdateMultipleAuthors()
        {
            PubContext _context = new();
            var LermanAuthors = _context.Authors.Where(a => a.LastName == "Lehrman").ToList();
            foreach (var la in LermanAuthors)
            {
                la.LastName = "Lerman";
            }
            //note: \r\n is unicode to get a new line instead of the long Environment.NewLine
            Console.WriteLine($"Before:\r\n{_context.ChangeTracker.DebugView.ShortView}");
            _context.ChangeTracker.DetectChanges();
            Console.WriteLine($"After:\r\n{_context.ChangeTracker.DebugView.ShortView}");
            _context.SaveChanges();
        }

        //VariousOperations();
        void VariousOperations()
        {
            PubContext _context = new();
            var author = _context.Authors.Find(2); //this is currently Josie Newf
            author.LastName = "Newfoundland";
            var newauthor = new Author { LastName = "Appleman", FirstName = "Dan" };
            _context.Authors.Add(newauthor);
            _context.SaveChanges();
        }

        //CoordinatedRetrieveAndUpdateAuthor();
        void CoordinatedRetrieveAndUpdateAuthor()
        {
            var author = FindThatAuthor(3);
            if (author?.FirstName == "Julie")
            {
                author.FirstName = "Julia";
                SaveThatAuthor(author);
            }
        }

        Author FindThatAuthor(int authorId)
        {
            using var shortLivedContext = new PubContext();
            return shortLivedContext.Authors.Find(authorId);
        }

        void SaveThatAuthor(Author author)
        {
            using var anotherShortLivedContext = new PubContext();
            anotherShortLivedContext.Authors.Update(author);
            anotherShortLivedContext.SaveChanges();
        }

        //DeleteAnAuthor();
        void DeleteAnAuthor()
        {
            PubContext _context = new();
            var extraJL = _context.Authors.Find(1);
            if (extraJL != null)
            {
                _context.Authors.Remove(extraJL);
                _context.SaveChanges();
            }
        }

        //InsertMultipleAuthors();
        void InsertMultipleAuthors()
        {
            using var _context = new PubContext();
            var newAuthors = new Author[]{
               new Author { FirstName = "Ruth", LastName = "Ozeki" },
               new Author { FirstName = "Sofia", LastName = "Segovia" },
               new Author { FirstName = "Ursula K.", LastName = "LeGuin" },
               new Author { FirstName = "Hugh", LastName = "Howey" },
               new Author { FirstName = "Isabelle", LastName = "Allende" }
            };
            _context.AddRange(newAuthors);
            _context.SaveChanges();
        }

        //ExecuteDelete();
        void ExecuteDelete()
        {
            using var _context = new PubContext();
            var deleteId = 9;
            _context.Authors.Where(a => a.AuthorId == deleteId).ExecuteDelete();
            var startswith = "H";
            var count = _context.Authors.Where(a => a.LastName.StartsWith(startswith)).ExecuteDelete();
        }

        //ExecuteUpdate();
        void ExecuteUpdate()
        {
            using var _context = new PubContext();
            var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
            ////change price of books older than 10 years to $1.50
            var oldbookprice = 1.50m;
            _context.Books.Where(b => b.PublishDate < tenYearsAgo)
                .ExecuteUpdate(setters => setters.SetProperty(b => b.BasePrice, oldbookprice));

            ////change all last names to lower case
            _context.Authors
                .ExecuteUpdate(setters => setters.SetProperty(a => a.LastName, a => a.LastName.ToLower()));

            //change all last names back to title case
            //(note:May look funky but LINQ can't translate efforts like ToUpperInvariant and TextInfo)
            _context.Authors
                .ExecuteUpdate(setters => setters.SetProperty(
                    a => a.LastName,
                    a => a.LastName.Substring(0, 1).ToUpper() + a.LastName.Substring(1).ToLower()));
        }

        //Interacting with Related data
        //the data is also added for the realated data table after the data is inserted in the parent table
        //InsertNewAuthorWithBook();
        void InsertNewAuthorWithBook()
        {
            using var _context = new PubContext();
            var author = new Author { FirstName = "Lynda", LastName = "Rutledge" };
            author.Books.Add(new Book
            {
                Title = "West With Giraffes",
                PublishDate = new DateOnly(2021, 2, 1)
            });
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        //InsertNewAuthorWith2NewBooks();
        void InsertNewAuthorWith2NewBooks()
        {
            using var _context = new PubContext();
            var author = new Author { FirstName = "Don", LastName = "Jones" };
            author.Books.AddRange(new List<Book> {
                new Book {Title = "The Never", PublishDate = new DateOnly(2019, 12, 1) },
                new Book {Title = "Alabaster", PublishDate = new DateOnly(2019,4,1)}
            });
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        //AddNewBookToExistingAuthorInMemory();
        void AddNewBookToExistingAuthorInMemory()
        {
            using var _context = new PubContext();
            var author = _context.Authors.FirstOrDefault(a => a.LastName == "Howey");
            if (author != null)
            {
                author.Books.Add(
                  new Book { Title = "Wool", PublishDate = new DateOnly(2012, 1, 1) }
                  );
                //_context.Authors.Add(author); //this will cause a duplicate key error
            }
            _context.SaveChanges();
        }

        //AddNewBookToExistingAuthorInMemoryViaBook();
        void AddNewBookToExistingAuthorInMemoryViaBook()
        {
            using var _context = new PubContext();
            var book = new Book
            {
                Title = "Shift",
                PublishDate = new DateOnly(2012, 1, 1),
                AuthorId = 5 //no need to use find, directly add the id here and entity framework will take care of rest
            };
            //book.Author = _context.Authors.Find(5); //known id for Hugh Howey
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        //EagerLoadBooksWithAuthors();
        void EagerLoadBooksWithAuthors()
        {
            using var _context = new PubContext();
            //var authors=_context.Authors.Include(a=>a.Books).ToList();
            var pubDateStart = new DateOnly(2010, 1, 1);
            var authors = _context.Authors
                .Include(a => a.Books
                               .Where(b => b.PublishDate >= pubDateStart)
                               .OrderBy(b => b.Title))
                .ToList();

            authors.ForEach(a =>
            {
                Console.WriteLine($"{a.LastName} ({a.Books.Count})");
                a.Books.ForEach(b => Console.WriteLine($"     {b.Title}"));
            });
        }

        //Projections();
        void Projections()
        {
            using var _context = new PubContext();
            var unknownTypes = _context.Authors
                .Select(a => new
                {
                    a.AuthorId,
                    Name = a.FirstName.First() + "" + a.LastName,
                    a.Books  //.Where(b => b.PublishDate.Year < 2000).Count()
                })
                .ToList();
            var debugview = _context.ChangeTracker.DebugView.ShortView;
        }

        //ExplicitLoadCollection();
        void ExplicitLoadCollection()
        {
            using var _context = new PubContext();
            var author = _context.Authors.FirstOrDefault(a => a.LastName == "Howey");
            if (author != null)
            {
                _context.Entry(author).Collection(a => a.Books).Load();
            }
        }

        //FilterUsingRelatedData();
        void FilterUsingRelatedData()
        {
            using var _context = new PubContext();
            var recentAuthors = _context.Authors
                .Where(a => a.Books.Any(b => b.PublishDate.Year >= 2015))
                .ToList();
        }

        //ModifyingRelatedDataWhenTracked();
        void ModifyingRelatedDataWhenTracked()
        {
            using var _context = new PubContext();
            var author = _context.Authors.Include(a => a.Books)
                .FirstOrDefault(a => a.AuthorId == 5);
            //author.Books[0].BasePrice = (decimal)10.00;
            author.Books.Remove(author.Books[1]);
            //using this instead of save changes to update ef core change tracker
            _context.ChangeTracker.DetectChanges();
            var state = _context.ChangeTracker.DebugView.ShortView;
        }

        //ModifyingRelatedDataWhenNotTracked();
        void ModifyingRelatedDataWhenNotTracked()
        {
            using var _context = new PubContext();
            var author = _context.Authors.Include(a => a.Books)
                .FirstOrDefault(a => a.AuthorId == 5);
            author.Books[0].BasePrice = (decimal)12.00;

            var newContext = new PubContext();
            //newContext.Books.Update(author.Books[0]);
            newContext.Entry(author.Books[0]).State = EntityState.Modified;
            var state = newContext.ChangeTracker.DebugView.ShortView;
            newContext.SaveChanges();
        }

        //CascadeDeleteInActionWhenTracked();
        void CascadeDeleteInActionWhenTracked()
        {
            using var _context = new PubContext();
            //note : I knew that author with id 9 had books in my sample database
            var author = _context.Authors.Include(a => a.Books)
             .FirstOrDefault(a => a.AuthorId == 9);
            _context.Authors.Remove(author);
            var state = _context.ChangeTracker.DebugView.ShortView;
            _context.SaveChanges();
        }

        //ConnectExistingArtistAndCoverObjects();
        void ConnectExistingArtistAndCoverObjects()
        {
            using var _context = new PubContext();
            var artistA = _context.Artists.Find(1);
            var artistB = _context.Artists.Find(2);
            var coverA = _context.Covers.Find(1);
            coverA.Artists.Add(artistA);
            coverA.Artists.Add(artistB);
            _context.SaveChanges();
        }

        //CreateNewCoverWithExistingArtist();
        void CreateNewCoverWithExistingArtist()
        {
            using var _context = new PubContext();
            var artistA = _context.Artists.Find(1);
            var cover = new Cover { DesignIdeas = "Author has provided a photo" };
            cover.Artists.Add(artistA);
            _context.ChangeTracker.DetectChanges();
            _context.Covers.Add(cover);
            _context.SaveChanges();
        }

        //CreateNewCoverAndArtistTogether();
        void CreateNewCoverAndArtistTogether()
        {
            using var _context = new PubContext();
            var newArtist = new Artist { FirstName = "Kir", LastName = "Talmage" };
            var newCover = new Cover { DesignIdeas = "We like birds!" };
            newArtist.Covers.Add(newCover);
            _context.Artists.Add(newArtist);
            _context.SaveChanges();
        }

        //RetrieveAnArtistWithTheirCovers();
        void RetrieveAnArtistWithTheirCovers()
        {
            using var _context = new PubContext();
            var artistWithCovers = _context.Artists
                .Include(a => a.Covers)
                .FirstOrDefault(a => a.ArtistId == 1);
        }

        //RetrieveACoverWithItsArtists();
        void RetrieveACoverWithItsArtists()
        {
            using var _context = new PubContext();
            var coverWithArtists = _context.Covers
                .Include(c => c.Artists)
                .FirstOrDefault(c => c.CoverId == 1);
        }

        //RetrieveAllArtistsWithTheirCovers();
        void RetrieveAllArtistsWithTheirCovers()
        {
            using var _context = new PubContext();
            var artistsWithCovers = _context.Artists
                .Include(a => a.Covers).ToList();

            foreach (var a in artistsWithCovers)
            {
                Console.WriteLine($"{a.FirstName} {a.LastName}, Designs to work on:");
                var primaryArtistId = a.ArtistId;
                if (a.Covers.Count() == 0)
                {
                    Console.WriteLine("  No covers");
                }
                else
                {
                    foreach (var c in a.Covers)
                    {
                        string collaborators = "";
                        foreach (var ca in c.Artists.Where(ca => ca.ArtistId != primaryArtistId))
                        {
                            collaborators += $"{ca.FirstName} {ca.LastName}";
                        }
                        if (collaborators.Length > 0)
                        { collaborators = $"(with {collaborators})"; }
                        Console.WriteLine($"  *{c.DesignIdeas} {collaborators}");
                    }
                }
            }
        }

        //RetrieveAllArtistsWhoHaveCovers();
        void RetrieveAllArtistsWhoHaveCovers()
        {
            using var _context = new PubContext();
            var artistsWithCovers = _context.Artists
                .Where(a => a.Covers.Any()).ToList();
        }

        //UnAssignAnArtistFromACover();
        void UnAssignAnArtistFromACover()
        {
            using var _context = new PubContext();
            var coverwithartist = _context.Covers
                .Include(c => c.Artists.Where(a => a.ArtistId == 1))
                .FirstOrDefault(c => c.CoverId == 1);
            coverwithartist.Artists.RemoveAt(0);
            _context.ChangeTracker.DetectChanges();
            var debugview = _context.ChangeTracker.DebugView.ShortView;
            _context.SaveChanges();
        }


        void ReassignACover()
        {
            using var _context = new PubContext();
            var coverwithartist4 = _context.Covers
            .Include(c => c.Artists.Where(a => a.ArtistId == 4))
            .FirstOrDefault(c => c.CoverId == 5);
            coverwithartist4.Artists.RemoveAt(0);
            var artist3 = _context.Artists.Find(3);
            coverwithartist4.Artists.Add(artist3);
            _context.ChangeTracker.DetectChanges();
        }

        //GetAllBooksWithTheirCovers();
        void GetAllBooksWithTheirCovers()
        {
            using var _context = new PubContext();
            var booksandcovers = _context.Books.Include(b => b.Cover).ToList();
            booksandcovers.ForEach(book =>
             Console.WriteLine(
                 book.Title +
                 (book.Cover == null ? "--- No cover yet" : $"---  {book.Cover.DesignIdeas}")));

            //using projections
            var project  = _context.Books.Where(b=>b.Cover !=null)
                .Select(b=> new {b.Title, b.Cover.DesignIdeas}).ToList();


        }

        //GetAllBooksThatHaveCovers();
        void GetAllBooksThatHaveCovers()
        {
            using var _context = new PubContext();
            var booksandcovers = _context.Books.Include(b => b.Cover).Where(b => b.Cover != null).ToList();
            booksandcovers.ForEach(book =>
               Console.WriteLine(book.Title + ":" + book.Cover.DesignIdeas));
        }

        //ProjectBooksThatHaveCovers();
        void ProjectBooksThatHaveCovers()
        {
            using var _context = new PubContext();
            var anon = _context.Books.Where(b => b.Cover != null)
              .Select(b => new { b.Title, b.Cover.DesignIdeas })
              .ToList();
            anon.ForEach(b =>
              Console.WriteLine(b.Title + ": " + b.DesignIdeas));

        }

        //MultiLevelInclude();
        void MultiLevelInclude()
        {
            using var _context = new PubContext();
            var authorGraph = _context.Authors.AsNoTracking()
                .Include(a => a.Books)
                .ThenInclude(b => b.Cover)
                .ThenInclude(c => c.Artists)
                .FirstOrDefault(a => a.AuthorId == 4);

            Console.WriteLine();
            Console.WriteLine($"{authorGraph?.FirstName} {authorGraph?.LastName}");
            foreach (var book in authorGraph.Books)
            {
                Console.WriteLine($"Book: {book.Title}");
                if (book.Cover != null)
                {
                    Console.WriteLine($"Design Ideas: {book.Cover.DesignIdeas}");
                    Console.Write("Artist(s):");
                    book.Cover.Artists.ForEach(a => Console.Write($"{a.LastName} "));
                }
            }
        }

        //combing objects in one to one relationships
        //NewBookAndCover();
        void NewBookAndCover()
        {
            using var _context = new PubContext();
            var book = new Book
            {
                AuthorId = 1,
                Title = "Call Me Ishtar",
                PublishDate = new DateOnly(1973, 1, 1)
            };
            book.Cover = new Cover { DesignIdeas = "Image of Ishtar?" };
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        //AddCoverToExistingBook();
        void AddCoverToExistingBook()
        {
            using var _context = new PubContext();
            var book = _context.Books.Find(8); //Shift
            book.Cover = new Cover { DesignIdeas = "Rows and rows of freezers" };
            _context.SaveChanges();
        }

        //AddCoverToExistingBookThatHasAnUntrackedCover();
        void AddCoverToExistingBookThatHasAnUntrackedCover()
        {
            using var _context = new PubContext();
            //this will fail because theres a cover already in the database
            var book = _context.Books.Find(5); //The Never
            book.Cover = new Cover { DesignIdeas = "A spiral" };
            _context.SaveChanges();
        }

        //AddCoverToExistingBookWithTrackedCover();
        void AddCoverToExistingBookWithTrackedCover()
        {
            using var _context = new PubContext();
            var book = _context.Books.Include(b => b.Cover)
                                     .FirstOrDefault(b => b.BookId == 5); //The Never
            book.Cover = new Cover { DesignIdeas = "A spiral" };
            _context.ChangeTracker.DetectChanges();
            var debugview = _context.ChangeTracker.DebugView.ShortView;
        }

        //ProtectingFromUniqueFKSideEffects();
        void ProtectingFromUniqueFKSideEffects()
        {
            using var _context = new PubContext();
            var TheNeverDesignIdeas = "A spirally spiral";
            var book = _context.Books.Include(b => b.Cover)
                                     .FirstOrDefault(b => b.BookId == 5); //The Never
            if (book.Cover != null)
            {
                book.Cover.DesignIdeas = TheNeverDesignIdeas;
            }
            else
            {
                book.Cover = new Cover { DesignIdeas = TheNeverDesignIdeas };
            }
            _context.SaveChanges();
        }

        //SimpleRawSQL();
        void SimpleRawSQL()
        {
            using var _context = new PubContext();
            var authors = _context.Authors.FromSqlRaw("select * from authors")
                .Include(a => a.Books).ToList();
        }

        //ConcatenatedRawSql_Unsafe();   //There is no safe query with concatenated strings!
        void ConcatenatedRawSql_Unsafe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            var authors = _context.Authors
                .FromSqlRaw("SELECT * FROM authors WHERE lastname LIKE '" + lastnameStart + "%'")
                .OrderBy(a => a.LastName).TagWith("Concatenated_Unsafe").ToList();
            //tagWith with will add the commented text with our queries, this will show up in our logs and profilers
        }

        //FormattedRawSql_Unsafe();
        void FormattedRawSql_Unsafe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            var sql = String.Format("SELECT * FROM authors WHERE lastname LIKE '{0}%'", lastnameStart);
            var authors = _context.Authors.FromSqlRaw(sql)
                .OrderBy(a => a.LastName).TagWith("Formatted_Unsafe").ToList();
        }

        //FormattedRawSql_Safe();
        void FormattedRawSql_Safe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            var authors = _context.Authors
                .FromSqlRaw("SELECT * FROM authors WHERE lastname LIKE '{0}%'", lastnameStart)
                .OrderBy(a => a.LastName).TagWith("Formatted_Safe").ToList();
        }

        //StringFromInterpolated_Unsafe();
        void StringFromInterpolated_Unsafe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            string sql = $"SELECT * FROM authors WHERE lastname LIKE '{lastnameStart}%'";
            var authors = _context.Authors.FromSqlRaw(sql)
                .OrderBy(a => a.LastName).TagWith("Interpolated_Unsafe").ToList();
        }

        //StringFromInterpolated_StillUnsafe();
        void StringFromInterpolated_StillUnsafe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            var authors = _context.Authors
                .FromSqlRaw($"SELECT * FROM authors WHERE lastname LIKE '{lastnameStart}%'")
                .OrderBy(a => a.LastName).TagWith("Interpolated_StillUnsafe").ToList();
        }

        //StringFromInterpolated_Safe();
        void StringFromInterpolated_Safe()
        {
            using var _context = new PubContext();
            var lastnameStart = "L";
            var authors = _context.Authors
                .FromSql($"SELECT * FROM authors WHERE lastname LIKE '{lastnameStart}%'")
            .OrderBy(a => a.LastName).TagWith("Interpolated_Safe").ToList();
        }

#if false
StringFromInterpolated_SoSafeItWontCompile();
void StringFromInterpolated_SoSafeItWontCompile()
{
    var lastnameStart = "L";
    var sql = $"SELECT * FROM authors WHERE lastname LIKE '{lastnameStart}%'";
    var authors = _context.Authors.FromSql(sql)
    .OrderBy(a => a.LastName).TagWith("Interpolated_WontCompile").ToList();
}

FormattedWithInterpolated_SoSafeItWontCompile();
void FormattedWithInterpolated_SoSafeItWontCompile()
{
    var lastnameStart = "L";
    var authors = _context.Authors
        .FromSql
            ("SELECT * FROM authors WHERE lastname LIKE '{0}%'", lastnameStart)
        .OrderBy(a => a.LastName).TagWith("Interpolated_WontCompile").ToList();
}

#endif

        //RawSqlStoredProc();
        void RawSqlStoredProc()
        {
            using var _context = new PubContext();
            var authors = _context.Authors
                .FromSqlRaw("AuthorsPublishedinYearRange {0}, {1}", 2010, 2015)
                .ToList();
        }

        //InterpolatedSqlStoredProc();
        void InterpolatedSqlStoredProc()
        {
            using var _context = new PubContext();
            int start = 2010;
            int end = 2015;
            var authors = _context.Authors
            .FromSql($"AuthorsPublishedinYearRange {start}, {end}")
            .ToList();
        }

        //RunSqlQueryScalarMethods(); 
        void RunSqlQueryScalarMethods()
        {
            using var _context = new PubContext();
            var ids = _context.Database
            .SqlQuery<int>($"SELECT AuthorId FROM Authors").ToList();

            var titles = _context.Database
            .SqlQuery<string>($"SELECT Title FROM Books").ToList();

            var sometitles = _context.Database
             .SqlQuery<string>($"SELECT Title as VALUE FROM Books")
             .Where(t => t.Contains("The")).ToList();

            //var longtitles=_context.Database
            //.SqlQuery<string>($"SELECT Title as VALUE FROM Books")
            //.Where(t => t.Length > 10).ToList();//EF can't evalueate t.Length into SQL

            var longtitles = _context.Database
            .SqlQuery<string>($"SELECT Title FROM Books WHERE LEN(title)>{10}").ToList();

            var rawLongTitles = _context.Database
            .SqlQueryRaw<string>($"SELECT Title FROM Books WHERE LEN(title)>{0}", 10).ToList();

        }

        //RunSqlQueryNonEntityMethods();
        void RunSqlQueryNonEntityMethods()
        {
            using var _context = new PubContext();
            var xyz = _context.Database
                .SqlQuery<AuthorName>($"select lastname, firstname from authors").ToList();

            var xyz2 = _context.Database
                .SqlQuery<AuthorName>($"GetAuthorNames").ToList();

            //type can only be a class or record. it cannot be a struct

        }

        //GetAuthorsByArtist();
        void GetAuthorsByArtist()
        {
            using var _context = new PubContext();
            var authorartists = _context.AuthorsByArtist.ToList();
            var oneauthorartists = _context.AuthorsByArtist.FirstOrDefault();
            var Kauthorartists = _context.AuthorsByArtist
                                         .Where(a => a.Artist.StartsWith("K")).ToList();
            var debugView = _context.ChangeTracker.DebugView.ShortView;
        }

        //DeleteCover(9);
        void DeleteCover(int coverId)
        {
            using var _context = new PubContext();
            var rowCount = _context.Database.ExecuteSqlRaw("DeleteCover {0}", coverId);
            Console.WriteLine(rowCount);
        }

        //InsertNewAuthor();
        void InsertNewAuthor()
        {
            using var _context = new PubContext();
            var author = new Author { FirstName = "Madeline", LastName = "Miller" };
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        //InsertNewAuthorWithNewBook();
        void InsertNewAuthorWithNewBook()
        {
            using var _context = new PubContext();
            var author = new Author { FirstName = "Anne", LastName = "Enright" };
            author.Books.Add(new Book { Title = "The Green Road" });
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        class AuthorName
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
        }

    }
}