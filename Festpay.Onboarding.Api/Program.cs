using Carter;
using Festpay.Onboarding.Api.Middlewares;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Application.Modules;
using Festpay.Onboarding.Infra;
using Festpay.Onboarding.Infra.Context;
using Festpay.Onboarding.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAllOrigins",
        configurePolicy: policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://site.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddProblemDetails();
builder.Services.AddCarter();

AppModules.AddApplication(builder.Services);
builder.Services.AddSwagger(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<FestpayContext>().Database.Migrate();

if (builder.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}
else
{
    app.UseCors("AllowSpecificOrigins");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapCarter();
app.MapControllers();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

await app.RunAsync();
