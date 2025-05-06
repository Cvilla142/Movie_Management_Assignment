using Xunit;
using MovieManagement.Core;

namespace MovieManagement.Tests
{
    public class MovieBorrowerTests
    {
        [Fact]
        public void MovieBorrower_WhenAvailable_MarksUnavailable()
        {
            var manager = new MovieManager();
            var movie = new Movie
            {
                MovieId = "M1",
                Title = "Somethings Here",
                Director = "Dr Schahul",
                Genre = "Genre",
                ReleaseYear = 2020
                
            };


            manager.AddMovie(movie);

            var borrower = new MovieBorrower(manager);
            var user = new User("U1", "Alice", DateTime.Now);

            borrower.BorrowMovie("M1", user);

            Assert.False(movie.IsAvailable);
        }

        [Fact]
        public void BorrowMovie_WhenNotAvailable_EnqueuesUser()
        {
            var manager = new MovieManager();
            var movie = new Movie
            {
                MovieId = "I1",
                Title = "Somethings not",
                Director = "Dr Schahul",
                Genre = "Genre",
                ReleaseYear = 2020
            };
        manager.AddMovie(movie);
        
        var borrower = new MovieBorrower(manager);
        var firstUser = new User("A2", "Christian", DateTime.Now);
        borrower.BorrowMovie("I1", firstUser);

        var secondUser = new User("U2", "Ethan", DateTime.Now);

        borrower.BorrowMovie("I1", secondUser);

        var next = manager.DequeueNextWaitingUser("I1");
        Assert.Equal ("U2", next?.UserId);

        }


    }
}