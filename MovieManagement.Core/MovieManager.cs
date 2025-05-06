namespace MovieManagement.Core;

public class MovieManager
{
    private LinkedList<Movie> _movies = new();
    private Dictionary<string, Movie> _movieLookup = new();
    private Dictionary<string, Queue<User>> _waitingLists = new();

    public bool AddMovie(Movie movie) 
    {
        if (_movieLookup.ContainsKey(movie.MovieId))
        {
            Console.WriteLine("A movie with this ID already exists.");
            return false;
        }
        _movies.AddLast(movie);
        _movieLookup[movie.MovieId] = movie;
        _waitingLists[movie.MovieId] = new Queue<User>(); 
        Console.WriteLine("Movie was added");
        return true;
    }

    public bool RemoveMovie(string movieId)
    {
        if (!_movieLookup.ContainsKey(movieId))
        {
            Console.WriteLine("Movie not found.");
            return false;
        }
        var movieToRemove = _movieLookup[movieId];
        _movieLookup.Remove(movieId);
        _movies.Remove(movieToRemove);
        _waitingLists.Remove(movieId);
        return true;
    }

    public Movie? SearchByID(string movieId) 
    { 
        _movieLookup.TryGetValue(movieId, out var movie);
        return movie;
    }

    public List<Movie> SearchByTitle(string title) 
    { 
        List<Movie> results = new();
        foreach (var movie in _movies)
        {
            if (movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(movie);
            }
        }
        return results;
    }

    public void SortByTitle()
    {
        var sorted = MovieSorter.BubbleSortByTitle(_movies);
        _movies = new LinkedList<Movie>(sorted);
    }

    public void SortByReleaseYear()
    {
        var sorted = MovieSorter.InsertionSortByYear(_movies);
        _movies = new LinkedList<Movie>(sorted);
    }

    public void EnqueueWaitingUser(string movieId, User user)
    {
        if (!_waitingLists.ContainsKey(movieId))
        {
            _waitingLists[movieId] = new Queue<User>();
        }

        _waitingLists[movieId].Enqueue(user);
    }

    public User? DequeueNextWaitingUser(string movieId)
    {
        if (_waitingLists.ContainsKey(movieId) && _waitingLists[movieId].Count > 0)
        {
            return _waitingLists[movieId].Dequeue();
        }
        return null;
    }

    public void ExportToJson(string filepath)
    {
        // Implementation placeholder
    }

    public void ImportFromJson(string filepath)
    {
        // Implementation placeholder
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        return _movies;
    }
}