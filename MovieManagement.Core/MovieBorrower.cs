using System.IO;
using System.Linq;
using System.Text.Json;

namespace MovieManagement.Core
{
    public class MovieBorrower
    {
        private readonly MovieManager _movieManager;

        public MovieBorrower(MovieManager movieManager)
        {
            _movieManager = movieManager;
        }

        public void BorrowMovie(string movieId, User user)
        {
            var movie = _movieManager.SearchByID(movieId);
            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            if (movie.IsAvailable)
            {
                movie.IsAvailable = false;
                Console.WriteLine($"Movie '{movie.Title}' has been borrowed by user {user.Name}.");
            }
            else
            {
                _movieManager.EnqueueWaitingUser(movieId, user);
                Console.WriteLine($"Movie '{movie.Title}' is not available. User {user.Name} has been added to the waiting list.");
            }
        }

        public User? ReturnMovie(string movieId)
        {
            var movie = _movieManager.SearchByID(movieId)
                        ?? throw new KeyNotFoundException($"Movie with ID '{movieId}' not found.");

            // Dequeue next waiting user, if any
            var nextUser = _movieManager.DequeueNextWaitingUser(movieId);
            if (nextUser != null)
            {
                // Immediately assign to them
                movie.IsAvailable = false;
                return nextUser;
            }
            else
            {
                // No one waiting → mark available
                movie.IsAvailable = true;
                return null;
            }
        }
    }
}
