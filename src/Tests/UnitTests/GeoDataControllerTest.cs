using Moq;

using GeoHub.Controllers;
using GeoHub.Entities;
using GeoHub.Services;
using GeoHub.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests;

[TestClass]
public sealed class GeoDataControllerTest
{

    [TestMethod]
    public async Task GetCountries_ShouldReturn_StatusCode_500()
    {
         // Arrange
        var mockRepo = new Mock<IGeoDataService>();
        mockRepo.Setup(repo => repo.GetCountries()).Throws(new Exception());
        var controller = new GeoDataController(mockRepo.Object);

        // Act
        var result = await controller.GetCountries();

        // Assert
        Assert.IsInstanceOfType<StatusCodeResult>(result);
        Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
    }

    [TestMethod]
    public async Task GetCountries_ShouldReturn_Ok_WithoutObjects()
    {
        //Prepare
        var countries = new List<Country>();

         // Arrange
        var mockRepo = new Mock<IGeoDataService>();
        mockRepo.Setup(repo => repo.GetCountries())
            .ReturnsAsync(countries);
        var controller = new GeoDataController(mockRepo.Object);

        // Act
        var result = await controller.GetCountries();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(result);
        //Assert.AreEqual(0, ((List<string>?)((ObjectResult)result).Value)?.Count);
    }

    [TestMethod]
    public async Task GetCountries_ShouldReturn_Ok_WithObjects()
    {
        //Prepare
        var countries = new List<Country>();
        countries.Add(new (){CountryCode = "mm", CountryName = "Myanmar"});
        countries.Add(new (){CountryCode = "us", CountryName = "United States"});

         // Arrange
        var mockRepo = new Mock<IGeoDataService>();
        mockRepo.Setup(repo => repo.GetCountries())
            .ReturnsAsync(countries);
        var controller = new GeoDataController(mockRepo.Object);

        // Act
        var result = await controller.GetCountries();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(result);
        Assert.AreEqual(2, ((List<(string,string)>?)((ObjectResult)result).Value)?.Count);
    }
}
