using Microsoft.EntityFrameworkCore;
using GeoHub.Entities;


namespace GeoHub.Data;


public class GeoHubContext : DbContext, IGeoHubContext
{
    public GeoHubContext(DbContextOptions<GeoHubContext> options)
        : base(options)
    {

    }

    public DbSet<Country> Countries { get; set; } = null!;

    public DbSet<JwtToken> JwtTokens { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-one relationship
        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.RefreshToken)
        //     .WithOne(rt => rt.User)
        //     .HasForeignKey<RefreshToken>(rt => rt.User.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.JwtToken)
            .WithOne(rt => rt.User)
            .HasForeignKey<JwtToken>("User_Id"); 

        base.OnModelCreating(modelBuilder);
    }


    public async Task<User> GetUserWithReferenceToken(int userId)
    {
        var user = await Users
            .Include(u => u.JwtToken)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }


    public async Task SaveRefreshToken(JwtToken refreshToken)
    {
        var user = await Users
            .FirstOrDefaultAsync(u => u.Id == refreshToken.User.Id);
        await SaveChangesAsync();
    }
}