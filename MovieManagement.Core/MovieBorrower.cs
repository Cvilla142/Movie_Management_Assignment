using System;
using MovieManagement.Core;

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
                throw new KeyNotFoundException($"Sorry {movieId} is not found.");
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

        public void ReturnMovie(string movieId)
        {
            var movie = _movieManager.SearchByID(movieId);
            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID '{movieId} not found.");

            User? nextUser = _movieManager.DequeueNextWaitingUser(movieId);

            if (nextUser != null)
            {
                Console.WriteLine($"Movie '{movie.Title}' is now borrowed by next user in queue: {nextUser.Name}.");
                // Still marked as unavailable because it goes to next user
            }
            else
            {
                movie.IsAvailable = true;
                Console.WriteLine($"Movie '{movie.Title}' has been returned and is now available.");
            }
        }
    }
}
