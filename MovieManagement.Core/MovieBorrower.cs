using system   
using System.Collections.Generic;

public class MovieBorrower
{
    private readonly MovieManager _movieManager;

    public MovieBorrower(MovieManager _movieManager)
    {
        _movieManager = _movieManager;
    }

    public bool BorrowMovie(string movieId, string userId)
    {
        var movie = _movieManager.searchById(movieId);
        if (movie == null) return false;

        if (movie.IsAvailable)
        {
            movie.IsAvailable = false;
            return true;
        }
        else 
        {
            _movieManager.EnqueueWaitingUser(movieId, userId);
            return false;
        }

        var nextUser = _movieManager.DeQueueNextWaitingUser(movieId);
        if (nextUser != null)
        {
            assignedTo = nextUser;
        }
        else 
        {
            movie.IsAvailable = true;
        }
        return true;
    }


}