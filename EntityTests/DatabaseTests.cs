using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

namespace EntityTests
{
    [TestClass]
    internal class DatabaseTests
    {
        [TestMethod]
        public void CanInsertAuthorIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubTestData");

            using (var context = new PubContext(builder.Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var author = new Author { FirstName = "a", LastName = "b" };
                context.Authors.Add(author);
                //Debug.WriteLine($"Before save: {author.AuthorId}");
                context.SaveChanges();
                //Debug.WriteLine($"After save: {author.AuthorId}");

                Assert.AreNotEqual(0, author.AuthorId);
            }

        }

        //by not calling savechanges we can significantly decrease the time taken for out test
        [TestMethod]
        public void ChangeTrackerIdentifiesAddedAuthor()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubTestData");
            using var context = new PubContext(builder.Options);
            var author = new Author { FirstName = "a", LastName = "b" };
            context.Authors.Add(author);
            Assert.AreEqual(EntityState.Added, context.Entry(author).State);
        }
    }
}
