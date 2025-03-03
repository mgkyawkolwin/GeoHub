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
    public async Task<IActionResult> Authenticate([FromBody] Login login)
    {
        var user = await _authService.Authenticate(login);
        if (user != null)
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
}