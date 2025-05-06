namespace MovieManagement.Core;

public class User
{
    public string UserId { get; set;}
    public string Name { get; set;}
    public DateTime RegisteredDate { get; set;}

    public User(string userId, string name, DateTime registeredDate)
    {
        UserId = userId;
        Name = name;
        RegisteredDate = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Name}" ({UserId});
    }
}