using StockResearchPlatform.Commands;
using StockResearchPlatform.Models.PolygonModels;

namespace StockResearchPlatform.Services.Polygon
{
	public class PolygonDividendService
	{
		public const string DIVIDENDS_ENDPOINT = "/v3/reference/dividends";

		private readonly IConfiguration _configuration;
		private readonly PolygonBaseService _polygonBaseService;
		private readonly string? _apiKey;
		private readonly string? _baseUrl;

		public PolygonDividendService(IConfiguration configuration, PolygonBaseService polygonBaseService)
		{
			_configuration = configuration;
			_polygonBaseService = polygonBaseService;
			_apiKey = _configuration.GetSection("PolygonAPI")["Key"];
			_baseUrl = _configuration.GetSection("PolygonAPI")["BaseUrl"];
		}

		public async Task<DividendsV3Jto?> DividendsV3(DividendsReqCommand queryParams)
		{
			var dividendsV3Url = queryParams.BuildQueryParams(_baseUrl, DIVIDENDS_ENDPOINT, _apiKey);

			return await _polygonBaseService.GetJto<DividendsV3Jto>(dividendsV3Url);
		}
	}
}
