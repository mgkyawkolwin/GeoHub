namespace GeoHub.Entities;

public class JwtToken : BaseEntity
{
    public string? AccessToken { get; set;}

    public DateTime ExpiresIn { get; set; }

    public string? RefreshToken { get; set;}

    public string? TokenType { get; set;}

    public User? User { get; set; }

}