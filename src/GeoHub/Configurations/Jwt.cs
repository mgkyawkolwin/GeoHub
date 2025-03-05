using GeoHub.Common;

namespace GeoHub.Configurations;

public class Jwt 
{
    private readonly IConfiguration _configuration;

    public Jwt(IConfiguration configuration)
    {
        _configuration = configuration;
        Audience = _configuration[AppSettingKeys.Jwt.Audience];
        ExpiryInMinutes = Convert.ToDouble(_configuration[AppSettingKeys.Jwt.ExpiryInMinutes]);
        Issuer = _configuration[AppSettingKeys.Jwt.Issuer];
        Key = _configuration[AppSettingKeys.Jwt.Key];
    }

    public string? Audience { get; set; }

    public double ExpiryInMinutes { get; set; }

    public string? Issuer { get; set; }

    public string? Key { get; set; }
}