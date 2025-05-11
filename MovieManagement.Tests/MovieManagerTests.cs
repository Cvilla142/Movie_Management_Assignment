using System;
using System.Collections.Generic;
using Xunit;
using MovieManagement.Core;

namespace MovieManagement.Tests
{
    public class MovieManagerTests
    {
        [Fact]
        public void AddMovie_ShouldAddMovieSuccessfully()
        {
            var manager = new MovieManager();
            var movie = new Movie { MovieId = "M1", Title = "Inception", Director = "Nolan", Genre = "Sci-Fi", ReleaseYear = 2010, IsAvailable = true };

            var added = manager.AddMovie(movie);
            var result = manager.SearchByID("M1");

            Assert.True(added);
            Assert.NotNull(result);
            Assert.Equal("Inception", result?.Title);
        }

        [Fact]
        public void AddMovie_WithDuplicateID_ShouldThrowException()
        {
            var manager = new MovieManager();
            var movie1 = new Movie { MovieId = "D1", Title = "Dune", Director = "Villeneuve", Genre = "Sci-Fi", ReleaseYear = 2021, IsAvailable = true };
            var movie2 = new Movie { MovieId = "D1", Title = "Dune 2", Director = "Smith", Genre = "Sci-Fi", ReleaseYear = 2023, IsAvailable = true };
            manager.AddMovie(movie1);

            Assert.Throws<InvalidOperationException>(() => manager.AddMovie(movie2));
        }

        [Fact]
        public void RemoveMovie_ShouldRemoveMovieSuccessfully()
        {

            var manager = new MovieManager();
            var movie = new Movie { MovieId = "M2", Title = "The Matrix", Director = "Wachowski", Genre = "Sci-Fi", ReleaseYear = 1999, IsAvailable = true };
            manager.AddMovie(movie);

            var removed = manager.RemoveMovie("M2");
            var result = manager.SearchByID("M2");

            Assert.True(removed);
            Assert.Null(result);
        }

        [Fact]
        public void RemoveMovie_NonExistentID_ReturnsFalse()
        {
            var manager = new MovieManager();

            var removed = manager.RemoveMovie("NON_EXISTENT");

            Assert.False(removed);
            Assert.Null(manager.SearchByID("NON_EXISTENT"));
        }

        [Fact]
        public void SearchByTitle_PartialMatch_ReturnsMatches()
        {
            var manager = new MovieManager();
            var m1 = new Movie { MovieId = "S1", Title = "Star Wars", Director = "Lucas", Genre = "Sci-Fi", ReleaseYear = 1977, IsAvailable = true };
            var m2 = new Movie { MovieId = "S2", Title = "Star Peace", Director = "Anderson", Genre = "Sci-Fi", ReleaseYear = 1997, IsAvailable = true };
            var m3 = new Movie { MovieId = "S3", Title = "Star Planet", Director = "Jones", Genre = "Sci-Fi", ReleaseYear = 1984, IsAvailable = true };
            var m4 = new Movie { MovieId = "S4", Title = "Alien", Director = "Scott", Genre = "Sci-Fi", ReleaseYear = 1979, IsAvailable = true };
            manager.AddMovie(m1);
            manager.AddMovie(m2);
            manager.AddMovie(m3);
            manager.AddMovie(m4);

            var results = manager.SearchByTitle("sTaR");

            Assert.Equal(3, results.Count);
            Assert.Contains(results, m => m.MovieId == "S1");
            Assert.Contains(results, m => m.MovieId == "S2");
            Assert.Contains(results, m => m.MovieId == "S3");
        }

        [Fact]
        public void SearchByID_ExactMatch_ReturnsCorrectMovie()
        {

            var manager = new MovieManager();
            var m1 = new Movie { MovieId = "ID123", Title = "TestTitle", Director = "Tester", Genre = "Demo", ReleaseYear = 2025, IsAvailable = true };
            var m2 = new Movie { MovieId = "ID456", Title = "OtherTitle", Director = "Tester", Genre = "Demo", ReleaseYear = 2020, IsAvailable = true };
            manager.AddMovie(m1);
            manager.AddMovie(m2);

            var result = manager.SearchByID("ID123");

            Assert.NotNull(result);
            Assert.Equal("TestTitle", result?.Title);
            Assert.Equal("ID123", result?.MovieId);
        }
    }
}
