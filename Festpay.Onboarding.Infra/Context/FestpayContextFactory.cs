using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Festpay.Onboarding.Infra.Context;

public class FestpayContextFactory : IDesignTimeDbContextFactory<FestpayContext>
{
    public FestpayContext CreateDbContext()
    {
        return CreateDbContext([]);
    }

    public FestpayContext CreateDbContext(string[] args)
    {
        //var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new Exception("Connection string not found");
        
        // SQLite
        var optionsBuilder = new DbContextOptionsBuilder<FestpayContext>();
        optionsBuilder
            .UseSqlite("festpay.db")
            .EnableSensitiveDataLogging(false)
            .EnableDetailedErrors(false);

        return new FestpayContext(optionsBuilder.Options);
    }
}
