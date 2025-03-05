namespace GeoHub.Configurations;

public interface IAppSettings
{
    string? Audience { get; set; }
    double ExpiryInMinutes {get;set;}
    string? Issuer { get; set; }
    string? Key { get; set; }
}