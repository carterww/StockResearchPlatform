using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Services;
using Hangfire;
using Hangfire.MySql;
using StockResearchPlatform.Services.DividendTracker;
using StockResearchPlatform.Services.Polygon;
using StockResearchPlatform.Repositories;
using StockResearchPlatform.Services.PortfolioComparison;

/**********************************************************
            CHANGE CONNECTION STRING HERE
**********************************************************/
const string CURRENT_CON_STRING_NAME = "ProductionConnection";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var connectionString = builder.Configuration.GetConnectionString(CURRENT_CON_STRING_NAME);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}, ServiceLifetime.Scoped);

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
#region Repositories
builder.Services.AddTransient<DividendInfoRepository>();
builder.Services.AddTransient<DividendLedgerRepository>();
#endregion

builder.Services.AddScoped<LoadStockDataToDatabaseService>();

builder.Services.AddScoped<AtomicBreakdownService>();
builder.Services.AddSingleton<HttpService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<StockSearchService>();
builder.Services.AddTransient<PortfolioService>();
builder.Services.AddTransient<StockService>();

#region PolygonServices
builder.Services.AddTransient<PolygonBaseService>();
builder.Services.AddTransient<PolygonTickerService>();
builder.Services.AddTransient<PolygonDividendService>();
builder.Services.AddTransient<IDividendTracker, DividendTracker>();
builder.Services.AddScoped<PortfolioComparisonService>();
#endregion

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
                        TransactionTimeout = TimeSpan.FromMinutes(5),
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
	using (var serviceScope = app.Services.CreateScope())
	{
		var dividendTracker = serviceScope.ServiceProvider.GetService<IDividendTracker>();
		dividendTracker.UpdateDividendInfoRecords();
	}
	return;
    //var breakdownService = app.Services.GetService<AtomicBreakdownService>();
    //await breakdownService?.BreakDownInvestment("VOO");
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
