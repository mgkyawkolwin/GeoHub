// using System.Data.Common;
// using Microsoft.AspNetCore;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.Hosting;
// // using Microsoft.Data.Sqlite;
// // using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
// // using RazorPagesProject.Data;
// using GeoHub.Data;

// namespace GeoHub.Tests.IntegrationTests;

// // <snippet1>
// public class CustomWebApplicationFactory<TProgram>
//     : WebApplicationFactory<TProgram> where TProgram : class
// {
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureServices(services =>
//         {
//             var dbContextDescriptor = services.SingleOrDefault(
//                 d => d.ServiceType == 
//                     typeof(IDbContextOptionsConfiguration<GeoHubContext>));

//             services.Remove(dbContextDescriptor);

//             var dbConnectionDescriptor = services.SingleOrDefault(
//                 d => d.ServiceType ==
//                     typeof(DbConnection));

//             services.Remove(dbConnectionDescriptor);

//             // Create open SqliteConnection so EF won't automatically close it.
//             services.AddSingleton<DbConnection>(container =>
//             {
//                 var connection = new SqliteConnection("DataSource=:memory:");
//                 connection.Open();

//                 return connection;
//             });

//             services.AddDbContext<GeoHubContext>((container, options) =>
//             {
//                 var connection = container.GetRequiredService<DbConnection>();
//                 options.UseSqlite(connection);
//             });
//         });

//         builder.UseEnvironment("Development");
//     }
// }
// // </snippet1>