using GeoHub.Entities;
using GeoHub.ServiceModels;

namespace GeoHub.Services;

public interface IAuthService
{
    Task<User> Authenticate(Login login);
}