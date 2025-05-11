using System;
using System.IO;
using System.Linq;
using Xunit;
using MovieManagement.Core;

namespace MovieManagement.Tests
{
    public class ImportExportTests
    {
        [Fact]
        public void ExportImport_RoundTrip_PreservesMoviesAndQueues()
        {
            var manager = new MovieManager();
            var movie = new Movie
            {
                MovieId = "R1",
                Title = "Test",
                Director = "Christian",
                Genre = "Action",
                ReleaseYear = 2025,
                IsAvailable = true
            };
            manager.AddMovie(movie);

            var borrower = new MovieBorrower(manager);
            var userA = new User("U1", "BorrowerA", DateTime.Now);
            borrower.BorrowMovie(movie.MovieId, userA);

            var userB = new User("U2", "QueuedUser", DateTime.Now);
            borrower.BorrowMovie(movie.MovieId, userB);


            var tempFile = Path.Combine(Path.GetTempPath(), $"movies_{Guid.NewGuid()}.json");
            try 
            {
                manager.ExportToJson(tempFile);

                var importedManager = new MovieManager();
                importedManager.ImportFromJson(tempFile);

                var imported = importedManager.SearchByID(movie.MovieId);
                Assert.NotNull(imported);
                Assert.Equal(movie.MovieId, imported.MovieId);
                Assert.Equal(movie.Title, imported.Title);
                Assert.Equal(movie.Director, imported.Director);
                Assert.Equal(movie.Genre, imported.Genre);
                Assert.Equal(movie.ReleaseYear, imported.ReleaseYear);

                Assert.False(imported.IsAvailable);

                var next = importedManager.DequeueNextWaitingUser(movie.MovieId);
                Assert.NotNull(next);
                Assert.Equal(userB.UserId, next.UserId);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }

        }
    }

}