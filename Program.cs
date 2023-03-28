using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var connectionString = builder.Configuration.GetConnectionString("ProductionConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}, ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<LoadStockDataToDatabaseService>();
builder.Services.AddSingleton<HttpService>();
builder.Services.AddSingleton<StockSearchService>();
builder.Services.AddTransient<PortfolioService>();
builder.Services.AddTransient<StockService>();

var app = builder.Build();

if (app.Environment.IsStaging())
{
    var loadDataService = app.Services.GetService<LoadStockDataToDatabaseService>();
    loadDataService?.LoadStocksToDatabase();
    loadDataService?.LoadMutualFundsToDatabase();
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
