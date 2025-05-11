using Xunit;
using MovieManagement.Core;

namespace MovieManagement.Tests
{
    public class MoveiManagerTests
    {
        [Fact]
        public void AddMovie_ShoudlAddMovieSuccesssfully()
        {
            var manager = new MovieManager();
            var movie = new Movie { MovieId = "M1", Title = "Inception", IsAvailable = true };


            manager.AddMovie(movie);
            var result = manager.SearchByID("M1");

            Assert.NotNull(result);
            Assert.Equal("Inception", result?.Title);
        }       

        [Fact]
        public void RemoveMovie_shouldRemoveMovieSuccessfully()
        {
            var manager = new MovieManager();
            var movie = new Movie { MovieId = "M2", Title = "The Matrix", IsAvailable = true };
            manager.AddMovie(movie);

            manager.RemoveMovie("M2");
            var result = manager.SearchByID("M2");

            Assert.Null(result);
        }

        [Fact]
        public void AddMovie_WithDuplicateID_ShouldThrowException()
        {
            var manager = new MovieManager();
            var movie1 = new Movie { MovieId = "D1", Title = "Dune" };
            var movie2 = new Movie { MovieId = "D1", Title = "Duplicated Dune" };

            manager.AddMovie(movie1);

            Assert.Throws<InvalidOperationException>(() => manager.AddMovie(movie2));
        }

        [Fact]
        public void RemoveMovie_NonExistentID_ReturnsFalse()
        {
            var manager = new MovieManager();

            var result = manager.RemoveMovie("Non_Existent_ID");


            Assert.False(result);
            Assert.Null(manager.SearchByID("Non_Existent_ID"));
        }

        [Fact]
        public void SearchByTitle_PartialMatch_ReturnsMatches()
        {
            var manager = new MovieManager();
            var m1 = new Movie { MovieId = "S1", Title = "Star Wars", Director = "D", Genre = "Sci-Fi", ReleaseYear = 1977, IsAvailable = true };
            var m2 = new Movie { MovieId = "S2", Title = "Star Peace", Director = "D", Genre = "Sci-Fi", ReleaseYear = 1997, IsAvailable = true };
            var m3 = new Movie { MovieId = "S3", Title = "Star Planet", Director = "D", Genre = "Sci-Fi", ReleaseYear = 1984, IsAvailable = true };
            var m4 = new Movie { MovieId = "S4", Title = "Alien", Director = "D", Genre = "Sci-Fi", ReleaseYear = 1979, IsAvailable = true };

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
        
        var m1 = new Movie { MovieId = "ID123", Title = "Title", Director = "Director", Genre = "Genre", ReleaseYear = 2025, IsAvailable = true };
        var m2 = new Movie { MovieId = "ID456", Title = "BTitle", Director = "CDirector", Genre = "DGenre", ReleaseYear = 2020, IsAvailable = true };

        manager.AddMovie(m1);
        manager.AddMovie(m2);

        var result = manager.SearchByID("ID123");

        Assert.NotNull(result);
        Assert.Equal("Title", result.Title);
        Assert.Equal("ID123", result?.MovieId);
        }

    }
}