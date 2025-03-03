
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

using GeoHub.Attributes;
using GeoHub.Services;
using GeoHub.ServiceModels;
using Microsoft.AspNetCore.Authorization;

namespace GeoHub.Controllers;

[Route("Api/CookieAuth")]
[ApiController]
public class CookieAuthController : ControllerBase
{
    private IGeoDataService _geoDataService;

    #pragma warning disable IDE0290
    public CookieAuthController(IGeoDataService geoDataService)
    {
        _geoDataService = geoDataService;
    }
    #pragma warning restore IDE0290


    [HttpPost("Authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authenticate(Login login)
    {
        try
        {
            if(login.UserName == "aaa" && login.Password == "bbb")
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, login.UserName),
                    new (ClaimTypes.Role, "ApiClients")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, JwtBearerDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);

                return Ok();
            }
            return BadRequest();
        }
        catch
        {
            return StatusCode(500);
        }
    }


    [HttpGet("Unauthenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Unauthenticate()
    {
        try
        {
             // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        catch
        {
            return StatusCode(500);
        }
    }
}