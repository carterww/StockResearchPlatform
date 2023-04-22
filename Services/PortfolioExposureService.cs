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
        AtomicBreakdownService _atomicBreakdownService;
        public PortfolioExposureService(ApplicationDbContext context, PolygonTickerService polygonTickerService, AtomicBreakdownService atomicBreakdownService)
        {
            _context = context;
            _polygonTickerService = polygonTickerService;
            _atomicBreakdownService = atomicBreakdownService;
        }
        // use polygon service to get the data
        public async Task<string?> GetSicDescription(string ticker)
        {
            var tickerDetailsV3Jto = await _polygonTickerService.TickerDetailsV3(ticker);
            if (tickerDetailsV3Jto.results.sic_description == null)
            {
                return "ETF";
            }
            else
            {
                return tickerDetailsV3Jto.results.sic_description;
            }
        }

        public async Task<Dictionary<string, double>> ConvertToPortfolioExposure(Dictionary<Stock, StockPortfolio> stocks)
        {
            Dictionary<string, double> result = new();
            foreach (var stock in stocks)
            {
                var queryParams = new PreviousCloseReqCommand { stocksTicker = stock.Key.Ticker, adjusted = true };
                PreviousCloseJto? previousClose = await _polygonTickerService.PreviousCloseV2(queryParams);

                var currentValue = previousClose.results[0].c;
                var sic = await GetSicDescription(stock.Key.Ticker);
                Console.WriteLine(sic);
                if (result.ContainsKey(sic))
                {
                    result[sic] += stock.Value.NumberOfShares * currentValue;
                }
                else
                {
                    result.Add(sic, stock.Value.NumberOfShares * currentValue);
                }
            }

            return result;
        }

        /*public async Task<Dictionary<string, double>> ConvertToAtomicPortfolioExposure(Dictionary<Stock, StockPortfolio> stocks)
        {
            var data = await _atomicBreakdownService.AtomicBreakdown(stocks);
            Dictionary<string, double> result = new();
            foreach (var kv in data)
            {
                var sic = await GetSicDescription(kv.Key);
                if (result.ContainsKey(sic))
                {
                    result[sic] += kv.Value;
                }
                else
                {
                    result.Add(sic, kv.Value);
                }
            }
        } */
    }
}
