using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO; // Make sure this is included

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // This will get the base path of the application's startup project.
            // When running `dotnet ef`, it automatically uses the startup project's directory.
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // optional: false ensures it fails if not found
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Make sure "CadenaSQL" is correctly defined in your appsettings.json
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}