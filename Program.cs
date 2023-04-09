using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Services;
using Hangfire;
using Hangfire.MySql;
using StockResearchPlatform.Services.DividendTracker;
using StockResearchPlatform.Services.Polygon;
using StockResearchPlatform.Repositories;

/**********************************************************
            CHANGE CONNECTION STRING HERE
**********************************************************/
const string CURRENT_CON_STRING_NAME = "CarterConnection";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var connectionString = builder.Configuration.GetConnectionString(CURRENT_CON_STRING_NAME);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}, ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
#region Repositories
builder.Services.AddTransient<DividendInfoRepository>();
builder.Services.AddTransient<DividendLedgerRepository>();
#endregion

builder.Services.AddScoped<LoadStockDataToDatabaseService>();
builder.Services.AddSingleton<HttpService>();
builder.Services.AddSingleton<StockSearchService>();
builder.Services.AddTransient<PortfolioService>();
builder.Services.AddTransient<StockService>();
#region PolygonServices
builder.Services.AddTransient<PolygonBaseService>();
builder.Services.AddTransient<PolygonTickerService>();
builder.Services.AddTransient<PolygonDividendService>();
#endregion
builder.Services.AddTransient<IDividendTracker, DividendTracker>();

builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(
                new MySqlStorage(
					builder.Configuration.GetConnectionString(CURRENT_CON_STRING_NAME),
                    new MySqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(10),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 25000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire",
                    }
                )
            ));

builder.Services.AddHangfireServer(options => options.WorkerCount = 1);

var app = builder.Build();

if (app.Environment.IsStaging())
{
	//var loadDataService = app.Services.GetService<LoadStockDataToDatabaseService>();
	//loadDataService?.LoadStocksToDatabase();
	//loadDataService?.LoadMutualFundsToDatabase();
	var dividendTrackerLOL = app.Services.GetService<IDividendTracker>();
    dividendTrackerLOL.UpdateDividendInfoRecords();
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

app.UseHangfireDashboard();

var scope = app.Services.CreateScope();
using (var serviceScope = app.Services.CreateScope())
{
	var dividendTracker = serviceScope.ServiceProvider.GetService<IDividendTracker>();

	RecurringJob.AddOrUpdate("DividendUpdate", () => dividendTracker.UpdateDividendInfoRecords(), "0 4 * * *");
}

app.Run();
