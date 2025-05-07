using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace MovieManagement.Core;

public class MovieManager
{
    private LinkedList<Movie> _movies = new();
    private Dictionary<string, Movie> _movieLookup = new();
    private Dictionary<string, Queue<User>> _waitingLists = new();

    public bool AddMovie(Movie movie) 
    {
        if (_movieLookup.ContainsKey(movie.MovieId))
            throw new InvalidOperationException("A movie witht his ID already exists.");

        _movies.AddLast(movie);
        _movieLookup[movie.MovieId] = movie;
        _waitingLists[movie.MovieId] = new Queue<User>(); 
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

    public void ExportToJson(string filePath)
    {

        var exportList = _movies
            .Select(m => new
            {
                m.MovieId,
                m.Title,
                m.Director,
                m.Genre,
                m.ReleaseYear,
                m.IsAvailable,
                WaitingQueue = _waitingLists[m.MovieId].Select(u => u.UserId).ToList()
            })
            .ToList();


        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(exportList, options);

        File.WriteAllText(filePath, json);
    }

    public void ImportFromJson(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Import file not found: {filePath}");


        string json = File.ReadAllText(filePath);

        var imported = JsonSerializer.Deserialize<List<ImportModel>>(json);
        if (imported == null)
            imported = new List<ImportModel>();


        _movies.Clear();
        _movieLookup.Clear();
        _waitingLists.Clear();


        foreach (var item in imported)
        {
            // Create movie object
            var mv = new Movie(
                item.MovieId,
                item.Title,
                item.Director,
                item.Genre,
                item.ReleaseYear
            )
            {
                IsAvailable = item.IsAvailable
            };


            AddMovie(mv);

            foreach (var uid in item.WaitingQueue)
            {
                _waitingLists[mv.MovieId].Enqueue(new User(uid, uid, DateTime.MinValue));
            }
        }
    }

    private class ImportModel
    {
        public string MovieId     { get; set; } = "";
        public string Title       { get; set; } = "";
        public string Director    { get; set; } = "";
        public string Genre       { get; set; } = "";
        public int    ReleaseYear { get; set; }
        public bool   IsAvailable { get; set; }
        public List<string> WaitingQueue { get; set; } = new();
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        return _movies;
    }
}