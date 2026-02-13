using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Festpay.Onboarding.Infra;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<FestpayContext>(options =>
            options
                .UseSqlite(config.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging(false)
                .EnableDetailedErrors(false)
                .ConfigureWarnings(w =>
                    w.Ignore(RelationalEventId.PendingModelChangesWarning)));
    }

    public async static Task RunMigrations(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
            await scope.ServiceProvider.GetRequiredService<FestpayContext>().Database.MigrateAsync();
    }
}
