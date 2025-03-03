using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using AngleSharp;
using Xunit;

namespace GeoHub.Tests.IntegrationTests;

public class GeoDataController_BasicTest 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GeoDataController_BasicTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("api/geodata/getcountries")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
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
}
