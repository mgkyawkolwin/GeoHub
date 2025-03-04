using Microsoft.EntityFrameworkCore;
using GeoHub.Entities;


namespace GeoHub.Data;

public interface IGeoHubContext
{
    DbSet<Country> Countries { get; set; }
    DbSet<JwtToken> JwtTokens { get; set; }
    DbSet<User> Users { get; set; }

    Task<User> GetUserWithReferenceToken(int userId);
    Task SaveRefreshToken(JwtToken refreshToken);
}