using GeoHub.Common;

namespace GeoHub.Configurations;

/// <summary>
/// Provide values from appsettings.json
/// </summary>
public class AppSettings
{

    public AppSettings(Jwt jwt)
    {
        Jwt = jwt;
    }

    public Jwt Jwt { get; set; }
    
}