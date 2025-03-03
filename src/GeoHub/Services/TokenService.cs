using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using GeoHub.Configurations;
using GeoHub.Entities;

namespace GeoHub.Services;

public class TokenService
{
    private readonly AppSettings _appSettings;

    public TokenService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
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

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenStr = GenerateRefreshToken();

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task SaveRefreshToken(User user, string refreshToken)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();
    }
}