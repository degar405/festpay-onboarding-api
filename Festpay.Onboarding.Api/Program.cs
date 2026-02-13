using Carter;
using Festpay.Onboarding.Api;
using Festpay.Onboarding.Api.Middlewares;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Application.Modules;
using Festpay.Onboarding.Infra;
using Festpay.Onboarding.Infra.Repositories;

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
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

var runMigrationTask = app.Services.RunMigrations();

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

await runMigrationTask;

await app.RunAsync();
