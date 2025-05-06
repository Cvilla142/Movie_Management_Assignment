using Xunit;
using MovieManagement.Core;


namespace MovieManagement.Tests
{
    public class MovieSorterTests
    {
        [Fact]
        public void BubbleSortByTitle_SortsAlphabetically()
        {

            var movies = new List<Movie>
            {
                new Movie { MovieId = "1", Title = "Boomboom", Director = "Christian", Genre = "Action", ReleaseYear = 2002 },
                new Movie { MovieId = "2", Title = "Something", Director = "Ethan", Genre = "Horror", ReleaseYear = 1998 },
                new Movie { MovieId = "3", Title = "DavinciCoder", Director = "Shawn", Genre = "Thriller", ReleaseYear = 2000 }
            };


            var sorted = MovieSorter.BubbleSortByTitle(movies);

            //What's supposed to happen
            var titles = sorted.Select(m => m.Title).ToList();
            Assert.Equal(new List<string> { "Boomboom", "DavinciCoder", "Something" }, titles);
        }

        [Fact]
        public void InsertionSortByYear_SortsByReleaseYear()
            {
                
                var movies = new List<Movie>
                {
                    new Movie { MovieId = "1", Title = "A", Director = "D", Genre = "G", ReleaseYear = 2030 },
                    new Movie { MovieId = "2", Title = "B", Director = "D", Genre = "G", ReleaseYear = 2021 },
                    new Movie { MovieId = "3", Title = "C", Director = "D", Genre = "G", ReleaseYear = 2060 }
                };

                var sorted = MovieSorter.InsertionSortByYear(movies);

                var years = sorted.Select(m => m.ReleaseYear).ToList();
                Assert.Equal(new List<int> { 2021, 2030, 2060 }, years);
            }
    }

    
}