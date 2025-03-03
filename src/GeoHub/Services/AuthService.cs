using Microsoft.EntityFrameworkCore;

using GeoHub.Data;
using GeoHub.Entities;
using GeoHub.ServiceModels;

namespace GeoHub.Services;

public class AuthService : IAuthService
{
    private readonly IGeoHubContext _geoHubContext;
    public AuthService(IGeoHubContext geoHubContext)
    {
        _geoHubContext = geoHubContext;
    }

    public async Task<User> Authenticate(Login login)
    {
        var user = await _geoHubContext.Users
            .FirstOrDefaultAsync(u => u.UserName == login.UserName && u.Password == login.Password);
        if (user != null)
            return user;

        return null;
    }
}