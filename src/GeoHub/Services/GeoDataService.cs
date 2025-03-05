using Microsoft.EntityFrameworkCore;

using GeoHub.Data;
using GeoHub.Entities;
using GeoHub.ServiceModels;


namespace GeoHub.Services;


public class GeoDataService : IGeoDataService
{
    private readonly GeoHubContext _dataContext;

    public GeoDataService(GeoHubContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Country>> GetCountries()
    {
        return await _dataContext.Countries.ToListAsync();
    }
}