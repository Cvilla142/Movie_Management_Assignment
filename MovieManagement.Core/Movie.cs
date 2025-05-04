namespace MovieManagement.Core;

public class Movie
{
    public string MovieId { get; set;}
    public string MovieTitle { get; set;}
    public string Director { get; set;}
    public string Genre { get; set;}
    public int ReleaseYear { get; set;}
    public bool IsAvailable { get; set;}
}
