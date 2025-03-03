using Microsoft.EntityFrameworkCore;
using GeoHub.Entities;


namespace GeoHub.Data;


public class GeoHubContext : DbContext, IGeoHubContext
{
    public GeoHubContext(DbContextOptions<GeoHubContext> options)
        : base(options)
    {

    }

    public DbSet<CountryEntity> Countries { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-one relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.RefreshToken)
            .WithOne(rt => rt.User)
            .HasForeignKey<RefreshToken>(rt => rt.User.Id);

        base.OnModelCreating(modelBuilder);
    }


    public async Task<User> GetUserWithReferenceToken(int userId)
    {
        var user = await Users
            .Include(u => u.RefreshToken)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }


    public async Task SaveRefreshToken(RefreshToken refreshToken)
    {
        var user = await Users
            .FirstOrDefaultAsync(u => u.Id == refreshToken.User.Id);
        await SaveChangesAsync();
    }
}