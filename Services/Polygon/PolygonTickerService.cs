using StockResearchPlatform.Models.PolygonModels;
using System.Runtime.InteropServices;
using System.Text.Json;
using StockResearchPlatform.Commands;

namespace StockResearchPlatform.Services.Polygon
{
    public class PolygonTickerService
    {
        public const string TICKER_DETAILS_ENDPOINT = "v3/reference/tickers/{0}";

        private readonly IConfiguration _configuration;
        private readonly PolygonBaseService _polygonBaseService;
        private readonly string? _apiKey;
        private readonly string? _baseUrl;

        public PolygonTickerService(IConfiguration configuration, PolygonBaseService polygonBaseService)
        {
            _configuration = configuration;
            _polygonBaseService = polygonBaseService;
            _apiKey = _configuration.GetSection("PolygonAPI")["Key"];
            _baseUrl = _configuration.GetSection("PolygonAPI")["BaseUrl"];
        }

        /// <summary>
        ///	Returns a TickerDetailsJto object that relates directly to the JSON returned by
        ///	https://polygon.io/docs/stocks/get_v3_reference_tickers__ticker .
        ///	Uses version 3 of Polygon's API
        /// </summary>
        /// <param name="Ticker">Ticker of stock</param>
        /// <param name="Date">Optional date parameter to grab details from SEC filings from that date</param>
        /// <returns>An object containing all data from Polygon's API endpoint</returns>
        public async Task<TickerDetailsV3Jto?> TickerDetailsV3(string Ticker, DateTime? Date = null)
        {
            var tickerDetailsV3Url = _baseUrl + TICKER_DETAILS_ENDPOINT + $"?apiKey={_apiKey}";
            if (Date != null)
            {
                tickerDetailsV3Url += $"&date={string.Format("{0:yyyy-MM-dd}", Date)}";
            }
            tickerDetailsV3Url = string.Format(tickerDetailsV3Url, Ticker);

            return await _polygonBaseService.GetJto<TickerDetailsV3Jto>(tickerDetailsV3Url);
        }

        public async Task<StockFinancialsVX?> StockFinancialsVX(StockFinancialsReqCommand queryParams)
        {

        }
    }
}
