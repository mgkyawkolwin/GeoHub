namespace GeoHub.Entities;

public class User : BaseEntity
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public JwtToken? JwtToken { get; set; }
    
}