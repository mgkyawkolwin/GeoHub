using Microsoft.EntityFrameworkCore;
using GeoHub.Entities;


namespace GeoHub.Data;

public interface IGeoHubContext
{
    DbSet<CountryEntity> Countries { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<User> Users { get; set; }

    Task<User> GetUserWithReferenceToken(int userId);
    Task SaveRefreshToken(RefreshToken refreshToken);
}