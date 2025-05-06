namespace MovieManagement.Core;

public class MovieManager
{
    private LinkedList<Movie> movies = new();

    private Dictionary<string, Movie> movieLookup = new();

    public void Add AddMovie(Movie movie) 
    {
        if (_movieLookup.ContainsKey(movie.MovieID))
        {
            throw new InvalidOperationException("A Movie with this ID has already been added")
        }
        _movies.AddLast(movie);
        _movieLookup[movie.MovieID] = movie;
        _waitingLists[movie.MovieID] = new Queue<string>();
        Console.WriteLine("Movie was added");

    }
    
    
    public void RemoveMovie(Movie movie)
    {
        if (!_movieLookup.ContainsKey(movie.))
        {
            throw new KeyNotFoundException("Movie not created");
        }
        Movie movieToRemove = _movieLookup[movie.MovieID];
        _movieLookip.Remove(movieID);
        _movies.Remove(movieToRemove);
        _waitingLists.Remove(movidID);
    }
    
    public Movie>? SearchByID(string movieID) 
    { 
        if (_movieLookup.TryGetValue(movieID, out var movie))
        {
            return movie;
        }
        return null;
    }


    public List<Movie> SearchByTitle(string title) 
    { 
        List<Movie> results = new();
        foreach (var movie in _movies)
        {
            if (movie.Title.Contains(title, String StringComparison.OrdinalIgnoreCase))
            {
                results.add(movie);
            }
        }
        return results;
    }


    public void SortByTitle()
    {
        var sorted = MovieSorter.BubbleSortByTitle(_movies);
        _movies = new LinkedList<Movie>(sorted);
    }

    public void SortByReleaseYear();
    {
        var sorted = MovieSorter.InsertionSortByYear(_movies);
        _movies = new LinkedList<Movie>(sorted);
    }


    public void EnqueueWaitingUser(string movieId, string userId)
    {
        if (!_waitingLists.ContainsKey(movieId))
        {
            _waitingLists[MovieId] = new Queue<string>();
        }

        _waitingLists[movieId].Enqueue(userId)
    }

    public string? DequeueNextWaitingUser(string movieId)
    {
        if (_waitingLists.ContainsKey(movieId) && _waitingLists[movieId].Count > 0)
        {
            return _waitingLists[movieId].Dequeue();
        }
        return null;
    }

    public void ExportToJson(string filepath)
    {        
    }

    public void ImportToJson(string filepath,)
    {       
    }
}
