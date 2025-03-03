using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using AngleSharp;
using Xunit;

using GeoHub.Entities;

namespace GeoHub.Tests.IntegrationTests;

public class GeoDataController_JwtTest 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GeoDataController_JwtTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("api/geodata/getcountriessecured")]
    public async Task Get_EndpointsReturnFail(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        //response.Unauthorized(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Headers.Contains("WWW-Authenticate"));
    }

    [Theory]
    [InlineData("api/jwtauth/authenticate")]
    public async Task Post_EndpointsReturn_NotFound(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var jsonContent = new StringContent(
            "{\"UserName\":\"InvalidUserName\",\"Password\":\"WrongPassword\"}",
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await client.PostAsync(url, jsonContent);

        // Assert
        //response.Unauthorized(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("api/jwtauth/authenticate")]
    public async Task Post_EndpointsReturn_Success(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var jsonContent = new StringContent(
            "{\"UserName\":\"aaa\",\"Password\":\"aaa\"}",
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await client.PostAsync(url, jsonContent);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<dynamic>(content);
         Console.WriteLine("Hello World");
         Console.WriteLine(result.ToString());
        // Assert
        //response.Unauthorized(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Assert.True(response.Headers.Contains("Authorization"), response.Headers.ToString());
        Assert.False(string.IsNullOrEmpty(result.GetProperty("accessToken").GetString()));
        Assert.False(string.IsNullOrEmpty(result.GetProperty("refreshToken").GetString()));
    }
}
