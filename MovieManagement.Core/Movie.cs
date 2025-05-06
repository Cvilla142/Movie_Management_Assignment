using System.IO;
using System.Linq;
using System.Text.Json;


namespace MovieManagement.Core;


public class Movie
{
    public string MovieId { get; set; }
    public string Title { get; set; }             
    public string Director { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Movie() 
    { 
        MovieId = "";
        Title = "";
        Director = "";
        Genre = "";
        ReleaseYear = 0;

    }

    public Movie(string movieId, string title, string director, string genre, int releaseYear)
    {
        MovieId = movieId;
        Title = title;
        Director = director;
        Genre = genre;
        ReleaseYear = releaseYear;
    }

    public override string ToString()
    {
        return $"{Title} ({ReleaseYear}) - Directed by {Director}";
    }
}
