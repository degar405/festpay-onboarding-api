using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Festpay.Onboarding.Infra;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<FestpayContext>(options =>
            options
                .UseSqlite(config.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging(false)
                .EnableDetailedErrors(false));
    }

    public static void AddSwagger(this IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(c => c.LoadOpenApiOptions());
    }

    private static void LoadOpenApiOptions(this SwaggerGenOptions options)
    {
        var contact = new OpenApiContact() { Name = "Festpay Onboarding" };

        var info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Festpay Onboarding",
            Description = "API designed to manage the Festpay Onboarding application.",
            Contact = contact,
        };

        options.SwaggerDoc("v1", info);
    }
}
