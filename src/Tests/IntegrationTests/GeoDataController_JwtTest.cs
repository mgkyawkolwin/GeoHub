using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace GeoHub.Tests.IntegrationTests;

public class GeoDataController_JwtTest 
    : IClassFixture<CustomFixture>
{
    //private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly CustomFixture _customFixture;

    public GeoDataController_JwtTest(CustomFixture customFixture)
    {
        _customFixture = customFixture;
    }

    [Theory]
    [InlineData("api/geodata/getcountriessecured")]
    public async Task EndpointsReturnFail(string url)
    {
        // Arrange
        var client = _customFixture.Factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Headers.Contains("WWW-Authenticate"));
    }

    [Theory]
    [InlineData("api/jwtauth/authenticate")]
    public async Task EndpointsReturn_NotFound(string url)
    {
        // Arrange
        var client = _customFixture.Factory.CreateClient();
        var jsonContent = new StringContent(
            "{\"UserName\":\"InvalidUserName\",\"Password\":\"WrongPassword\"}",
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await client.PostAsync(url, jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("api/jwtauth/authenticate")]
    public async Task EndpointsReturn_Success(string url)
    {
        // Arrange
        var client = _customFixture.Factory.CreateClient();
        var jsonContent = new StringContent(
            "{\"UserName\":\"TestUser\",\"Password\":\"TestPassword\"}",
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await client.PostAsync(url, jsonContent);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<dynamic>(content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(result?.GetProperty("accessToken").GetString()));
        Assert.False(string.IsNullOrEmpty(result?.GetProperty("refreshToken").GetString()));

        // Request secured api
        var head = string.Format("Bearer {0};",
            result?.GetProperty("accessToken").GetString());
        client.DefaultRequestHeaders.Add("Authorization", "Bearer "+result?.GetProperty("accessToken").GetString());
        response = await client.GetAsync("api/geodata/getcountriessecured");
        content = await response.Content.ReadAsStringAsync();
        var countries = JsonSerializer.Deserialize<List<dynamic>>(content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, countries?.Count);
        Assert.Equal(countries?[0].GetProperty("countryCode").GetString(), "mm");

    }
}
