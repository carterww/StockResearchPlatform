using StockResearchPlatform.Models.PolygonModels;
using System.Runtime.InteropServices;
using System.Text.Json;
using StockResearchPlatform.Commands;

namespace StockResearchPlatform.Services.Polygon
{
    public class PolygonTickerService
    {
        public const string TICKER_DETAILS_ENDPOINT = "v3/reference/tickers/{0}";
        public const string STOCK_FINANCIALS_ENDPOINT = "vX/reference/financials";
        public const string TICKERS_ENDPOINT = "v3/reference/tickers";
        public const string PREVIOUS_CLOSE_ENDPOINT = "v2/aggs/ticker/{0}/prev";
        public const string DAILY_OPEN_CLOSE_ENDPOINT = "v1/open-close/{0}/{1}";
        public const string AGGREGATES_BARS_ENDPOINT = "v2/aggs/ticker/{0}/range/{1}/{2}/{3}/{4}";

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
            tickerDetailsV3Url = string.Format(tickerDetailsV3Url, Ticker.ToUpper());

            return await _polygonBaseService.GetJto<TickerDetailsV3Jto>(tickerDetailsV3Url);
        }


		/// <summary>
		/// Returns a StockFinancialsVXJto that relates directly to the JSON returned by
		/// https://polygon.io/docs/stocks/get_vx_reference_financials
		/// </summary>
		/// <param name="queryParams">StockFinancialsReqCommand used to build to the query parameters used for requesting the data from Polygon</param>
		/// <returns>An object containing all data from Polygon's API endpoint</returns>
		public async Task<StockFinancialsVXJto?> StockFinancialsVX(StockFinancialsReqCommand queryParams)
        {
            var stockFinancialsVXUrl = queryParams.BuildQueryParams(_baseUrl, STOCK_FINANCIALS_ENDPOINT, _apiKey);

            return await _polygonBaseService.GetJto<StockFinancialsVXJto>(stockFinancialsVXUrl);
        }

		/// <summary>
		/// Returns a TickerV3Jto that relates directly to the JSON returned by
		/// https://polygon.io/docs/stocks/get_v3_reference_tickers
		/// </summary>
		/// <param name="queryParams">TickersReqCommand used to build the query parameters used for requesting the data from Polygon</param>
		/// <returns>An object containing all data from Polygon's API endpoint</returns>
		public async Task<TickerV3Jto?> TickersV3(TickersReqCommand queryParams)
        {
            var tickersV3Url = queryParams.BuildQueryParams(_baseUrl, TICKERS_ENDPOINT, _apiKey);

            return await _polygonBaseService.GetJto<TickerV3Jto>(tickersV3Url);
        }

		/// <summary>
		/// Returns a PreviousCloseJto that relates directly to the JSON returned by
		/// https://polygon.io/docs/stocks/get_v2_aggs_ticker__stocksticker__prev
		/// </summary>
		/// <param name="queryParams">PreviousCloseReqCommand used to build the query parameters used for requesting</param>
		/// <returns>An object containing all data from Polygon's API endpoint</returns>
		public async Task<PreviousCloseJto?> PreviousCloseV2(PreviousCloseReqCommand queryParams)
        {
            var previousCloseV2Url = queryParams.BuildQueryParams(_baseUrl, PREVIOUS_CLOSE_ENDPOINT, _apiKey);

            return await _polygonBaseService.GetJto<PreviousCloseJto>(previousCloseV2Url);
        }

		/// <summary>
		/// Returns a DailyOpenCloseJto that relates diretly to the JSON returned by
		/// https://polygon.io/docs/stocks/get_v1_open-close__stocksticker___date
		/// </summary>
		/// <param name="queryParams">DailyOpenCloseReqCommand used to build the query parameters used for requesting</param>
		/// <returns>An object containing all data from Polygon's API endpoint</returns>
		public async Task<DailyOpenCloseJto?> DailyOpenCloseV1(DailyOpenCloseReqCommand queryParams)
        {
            var dailyOpenCloseV1Url = queryParams.BuildQueryParams(_baseUrl, DAILY_OPEN_CLOSE_ENDPOINT, _apiKey);

			return await _polygonBaseService.GetJto<DailyOpenCloseJto>(dailyOpenCloseV1Url);
		}
		/// <summary>
		/// Returns a AggregatesBarsV2Jto that relates diretly to the JSON returned by
		/// https://polygon.io/docs/stocks/get_v2_aggs_ticker__stocksticker__range__multiplier___timespan___from___to
		/// </summary>
		/// <param name="queryParams">AggregateBarsReqCommand used to build the query parameters used for requesting</param>
		/// <returns>An object containing all data from Polygon's API endpoint</returns>
		public async Task<AggregatesBarsV2Jto?> AggregatesBarsV2(AggregateBarsReqCommand queryParams)
        {
            var aggregatesBarsV2Url = queryParams.BuildQueryParams(_baseUrl, AGGREGATES_BARS_ENDPOINT, _apiKey);

            return await _polygonBaseService.GetJto<AggregatesBarsV2Jto>(aggregatesBarsV2Url);
        }
    }
}
