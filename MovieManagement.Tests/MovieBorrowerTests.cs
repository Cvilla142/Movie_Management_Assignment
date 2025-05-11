using System;
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
                MovieId     = "M1",
                Title       = "Somethings Here",
                Director    = "Dr Schahul",
                Genre       = "Genre",
                ReleaseYear = 2020,
                IsAvailable = true
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
                MovieId     = "I1",
                Title       = "Somethings not",
                Director    = "Dr Schahul",
                Genre       = "Genre",
                ReleaseYear = 2020,
                IsAvailable = true
            };
            manager.AddMovie(movie);

            var borrower = new MovieBorrower(manager);
            var firstUser = new User("A2", "Christian", DateTime.Now);
            borrower.BorrowMovie("I1", firstUser);

            var secondUser = new User("U2", "Ethan", DateTime.Now);

            borrower.BorrowMovie("I1", secondUser);

            var next = manager.DequeueNextWaitingUser("I1");
            Assert.Equal("U2", next?.UserId);
        }

        [Fact]
        public void ReturnMovie_WhenQueueNotEmpty_RemainsUnavailableAndQueueEmptied()
        {
            var manager = new MovieManager();
            var movie = new Movie
            {
                MovieId     = "L7",
                Title       = "Back for blood",
                Director    = "Christian",
                Genre       = "Action",
                ReleaseYear = 2021,
                IsAvailable = true
            };
            manager.AddMovie(movie);

            var borrower = new MovieBorrower(manager);
            var userA = new User("L2", "Schahul", DateTime.Now);
            var userB = new User("U2", "Jeli",   DateTime.Now);

            borrower.BorrowMovie("L7", userA);
            borrower.BorrowMovie("L7", userB);

            borrower.ReturnMovie("L7");

            Assert.False(movie.IsAvailable);
            Assert.Null(manager.DequeueNextWaitingUser("L7"));
        }

        [Fact]
        public void ReturnMovie_WhenQueueEmpty_MarksAvailable()
        {

            var manager = new MovieManager();
            var movie = new Movie
            {
                MovieId     = "L7",
                Title       = "Back for blood",
                Director    = "Christian",
                Genre       = "Action",
                ReleaseYear = 2021,
                IsAvailable = true
            };
            manager.AddMovie(movie);

            var borrower = new MovieBorrower(manager);
            var user = new User("I1", "Kenton", DateTime.Now);


            borrower.BorrowMovie("L7", user);
            borrower.ReturnMovie("L7");


            Assert.True(movie.IsAvailable);
        }
    }
}