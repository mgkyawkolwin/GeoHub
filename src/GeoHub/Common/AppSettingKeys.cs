
namespace GeoHub.Common;

/// <summary>
/// Provide the keys in the appsettings.json.
/// </summary>
public struct AppSettingKeys
{
    public struct Jwt 
    {
        public const string Audience = "Jwt:Audience";
        public const string ExpiryInMinutes = "Jwt:ExpiryInMinutes";
        public const string Key = "Jwt:Key";
        public const string Issuer = "Jwt:Issuer";
    
    }
}