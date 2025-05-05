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
    }
    

    public void SortByReleaseYear();
    {

    }

    public void BorrowMovie(string movieID, string userID) 
    {         
    }

    public void ReturnMovie(string filepath)
    {        
    }

    public void ExportToJson(string filepath)
    {        
    }

    public void ImportToJson(string filepath,)
    {       
    }
}
