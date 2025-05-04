namespace MovieManagement.Core;

public class MovieManager
{
    private LinkedList<Movie> movies = new();

    private Dictionary<string, Movie> movieLookup = new();

    public void Add AddMovie(Movie movie) 
    {
    }
    public void RemoveMovie(Movie movie)
    {
    }
    
    public Movie>? SearchByID(string movieID) { return null; }

    public List<Movie> SearchByTitle(string title) { return new List<Movie>(); }

    public void SortByTitle()
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
