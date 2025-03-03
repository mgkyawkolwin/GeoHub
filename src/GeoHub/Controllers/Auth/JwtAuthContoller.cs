using Microsoft.AspNetCore.Mvc;

using GeoHub.ServiceModels;
using GeoHub.Services;

namespace GeoHub.Controllers;


[Route("Api/[controller]")]
[ApiController]
public class JwtAuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly TokenService _tokenService;

    public JwtAuthController(IAuthService authService, TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("Authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authenticate([FromBody] Login login)
    {
        try
        {
            var user = await _authService.Authenticate(login);
            if (user != null)
            {
                var tokens = _tokenService.GenerateAllToken(login);
                return Ok(new  {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    Expires = tokens.Expires
                });
            }

            return NotFound();
        }catch 
        {
            return StatusCode(500);
        }
    }
}