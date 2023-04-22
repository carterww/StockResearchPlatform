using StockResearchPlatform.Commands;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Services
{
    public class PortfolioExposureService
    {
        ApplicationDbContext _context;
        PolygonTickerService _polygonTickerService;
        public PortfolioExposureService(ApplicationDbContext context, PolygonTickerService polygonTickerService)
        {
            _context = context;
            _polygonTickerService = polygonTickerService;
        }
        // use polygon service to get the data
        public async Task<string?> GetSicDescription(string ticker)
        {
            var tickerDetailsV3Jto = await _polygonTickerService.TickerDetailsV3(ticker);
            if (tickerDetailsV3Jto == null)
            {
                return "ETF";
            }
            else
            {
                return "Poo";
            }
        }

        public async Task<Dictionary<string, double>> ConvertToPortfolioExposure(Dictionary<Stock, StockPortfolio> stocks)
        {
            Dictionary<string, double> result = new();
            foreach (var stock in stocks)
            {
                var sic = await GetSicDescription(stock.Key.Ticker);
                Console.WriteLine(sic);
                if (result.ContainsKey(sic))
                {
                    result[sic] += stock.Value.NumberOfShares * stock.Value.CostBasis;
                }
                else
                {
                    result.Add(sic, stock.Value.NumberOfShares * stock.Value.CostBasis);
                }
            }

            return result;
        }
    }
}
