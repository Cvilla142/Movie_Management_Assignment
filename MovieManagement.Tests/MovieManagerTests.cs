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

            Assert.NotNUll(result);
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


    }
}