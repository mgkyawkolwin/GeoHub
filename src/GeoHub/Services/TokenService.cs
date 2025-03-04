using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using GeoHub.Configurations;
using GeoHub.Data;
using GeoHub.Entities;
using GeoHub.ServiceModels;

namespace GeoHub.Services;

public class TokenService
{
    private readonly AppSettings _appSettings;

    private readonly IGeoHubContext _geoHubContext;

    public TokenService(AppSettings appSettings, IGeoHubContext geoHubContext)
    {
        _appSettings = appSettings;
        _geoHubContext = geoHubContext;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateAccessToken(string userName)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _appSettings.Jwt.Issuer,
            audience: _appSettings.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_appSettings.Jwt.ExpiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string AccessToken,string RefreshToken, DateTime Expires) GenerateAllToken(Login login)
    {

        var tokenStr = GenerateAccessToken(login.UserName);
        var refreshTokenStr = GenerateRefreshToken();
        var expires = DateTime.UtcNow;

        return (tokenStr, refreshTokenStr, expires);
    }


    public async Task<(string AccessToken, string RefreshToken)> RefreshToken(string rToken)
    {
        var refreshToken = await _geoHubContext.JwtTokens
            .FirstOrDefaultAsync(rt => rt.RefreshToken == rToken);

        if (refreshToken == null || refreshToken.ExpiresIn < DateTime.UtcNow)
        {
            return default;
        }

        // Generate new access token
        var user = await _geoHubContext.Users.FirstOrDefaultAsync(u => u.JwtToken.RefreshToken == rToken);
        var newAccessToken = GenerateAccessToken(user.UserName);

        // Optionally, generate a new refresh token and revoke the old one
        var newRefreshToken = GenerateRefreshToken();
        refreshToken.RefreshToken = newRefreshToken;

        await SaveRefreshToken(user, newRefreshToken);

        return (newAccessToken, newRefreshToken);
    }


    public async Task RevokeToken(string rToken)
    {
        var refreshToken = await _geoHubContext.JwtTokens
            .FirstOrDefaultAsync(rt => rt.RefreshToken == rToken);

        if (refreshToken != null)
        {
            refreshToken.ExpiresIn = DateTime.UtcNow;
            _geoHubContext.JwtTokens.Update(refreshToken);
        }
    }
    
    private async Task SaveRefreshToken(User user, string refreshToken)
    {
        user.JwtToken.RefreshToken = refreshToken;
        user.JwtToken.ExpiresIn = DateTime.UtcNow.AddMinutes(20);

        _geoHubContext.Users.Update(user);
    }
}