using GeoHub.Entities;
using GeoHub.ServiceModels;

namespace GeoHub.Services;

public interface IGeoDataService
{
     Task<List<CountryEntity>> GetCountries();

}