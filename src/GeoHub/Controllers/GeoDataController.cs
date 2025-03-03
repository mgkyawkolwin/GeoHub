using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GeoHub.ServiceModels;
using GeoHub.Services;

namespace GeoHub.Controllers;

[Route("Api/GeoData")]
[ApiController]
public class GeoDataController : ControllerBase
{
    private IGeoDataService _geoDataService;

    #pragma warning disable IDE0290
    public GeoDataController(IGeoDataService geoDataService)
    {
        _geoDataService = geoDataService;
    }
    #pragma warning restore IDE0290


    [HttpGet("GetCountries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        try
        {
            var countries = await _geoDataService.GetCountries();
            var list = countries.Select(country => new Country{
                CountryName = country.CountryName,
                CountryCode = country.CountryCode
            }).ToList();
            return Ok(list);
        }
        catch
        {
            return StatusCode(500);
        }
    }


    [Authorize]
    [HttpGet("GetCountriesSecured")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountriesSecured()
    {
        try
        {
            var countries = await _geoDataService.GetCountries();
            var list = countries.Select(country => new Country{
                CountryName = country.CountryName,
                CountryCode = country.CountryCode
            }).ToList();
            return Ok(list);
        }
        catch
        {
            return StatusCode(500);
        }
    }
}