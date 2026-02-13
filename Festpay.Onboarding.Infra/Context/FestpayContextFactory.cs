using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Festpay.Onboarding.Infra.Context;

public class FestpayContextFactory : IDesignTimeDbContextFactory<FestpayContext>
{
    public FestpayContext CreateDbContext()
    {
        return CreateDbContext([]);
    }

    public FestpayContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "../Festpay.Onboarding.Api"
        );
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found");
        
        // SQLite
        var optionsBuilder = new DbContextOptionsBuilder<FestpayContext>();
        optionsBuilder
            .UseSqlite(connectionString);

        return new FestpayContext(optionsBuilder.Options);
    }
}
