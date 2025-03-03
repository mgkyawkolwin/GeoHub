namespace GeoHub.Entities;

public class RefreshToken : BaseEntity
{
    public DateTime Expires { get; set; }

    public string? Token { get; set;}

    public User? User { get; set; }

}