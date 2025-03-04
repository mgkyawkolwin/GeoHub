using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using AngleSharp;
using Xunit;

using GeoHub.Data;
using GeoHub.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GeoHub.Tests.IntegrationTests;

public class CustomFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> Factory { get; }
    public GeoHubContext DbContext { get; }

    public CustomFixture()
    {
        // Initialize the factory
        Factory = new CustomWebApplicationFactory<Program>();

        // Create a scope to resolve the DbContext
        var scope = Factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<GeoHubContext>();

        // Ensure the database is created and seeded
        //DbContext.Database.EnsureCreated();

        // Insert test data
        var testUser = new User
        {
            UserName = "TestUser",
            Password = "TestPassword"
        };

        var countries = new List<Country> {
            new() {
                CountryCode = "mm",
                CountryName = "Myanmar"
            },
            new (){
                CountryCode = "us",
                CountryName = "United States"
            }
        };

        DbContext.Users.Add(testUser);
        DbContext.Countries.AddRange(countries);
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        // Clean up the database
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
        Factory.Dispose();
    }
}