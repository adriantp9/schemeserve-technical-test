using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Data;
using System.Reflection;
using TechTest.Application.Clients;
using TechTest.Application.Services;
using TechTest.Constants;
using TechTest.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var baseUri = builder.Configuration["CqcClient:BaseUri"];
var apimSubscriptionKey = builder.Configuration["CqcClient:ApimSubscriptionKey"];

builder.Services.AddHttpClient<ICqcClient, CqcClient>(client =>
{
    client.BaseAddress = new Uri(baseUri!);
    client.DefaultRequestHeaders.Add(ApimConstants.SubscriptionKeyName, apimSubscriptionKey!);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

EnsureDatabase.For.SqlDatabase(connectionString);

var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));

// Application services.
builder.Services.AddTransient<ICqcProviderService, CqcProviderService>();

// Persistence repositories.
builder.Services.AddTransient<IProviderRepository, ProviderRepository>();

TechTest.Mapping.MapsterConfig.RegisterMappings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
