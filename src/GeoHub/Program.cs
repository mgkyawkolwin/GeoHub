using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
﻿using kDg.FileBaseContext.Extensions;

using GeoHub.Common;
using GeoHub.Data;
using GeoHub.Services;
using GeoHub.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<GeoHubContext>(options => options.UseFileBaseContextDatabase("GeoHubDb"));
builder.Services.AddDbContext<GeoHubContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectionString"]));

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
)
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration[AppSettingKeys.Jwt.Issuer],
        ValidAudience = builder.Configuration[AppSettingKeys.Jwt.Audience],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[AppSettingKeys.Jwt.Key]))
    };
});

// Register services
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddSingleton<Jwt>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddScoped<IGeoDataService, GeoDataService>();
builder.Services.AddScoped<IGeoHubContext, GeoHubContext>();


// Build the service provider
var sp = builder.Services.BuildServiceProvider();

// Ensure the database is created and migrated
using (var scope = sp.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    var db = scopedServices.GetRequiredService<GeoHubContext>();
    //db.Database.EnsureCreated();
    db.Database.Migrate();
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}



app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


// Required for integration test
public partial class Program { }