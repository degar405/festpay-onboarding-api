using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Festpay.Onboarding.Api.Extensions
{
    public static class SwaggerExtensions
    {
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
}
