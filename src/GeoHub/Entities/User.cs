namespace GeoHub.Entities;

public class User : BaseEntity
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public RefreshToken? RefreshToken { get; set; }
}