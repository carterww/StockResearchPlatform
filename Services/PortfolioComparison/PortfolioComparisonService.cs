using StockResearchPlatform.Commands;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Models.PortfolioDtos;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Services.PortfolioComparison
{
	// Alias for static class used for helping with calculations
	using Utils = PortfolioComparisonUtils;
	public class PortfolioComparisonService
	{
		private readonly PolygonTickerService _polygonTickerService;
		private readonly PortfolioService _portfolioService;

		public PortfolioComparisonService (
			PolygonTickerService polygonTickerService,
			PortfolioService portfolioService
			) 
		{ 
			_polygonTickerService = polygonTickerService;
			_portfolioService = portfolioService;
		}

		/// <summary>
		/// Compares two portfolio's returns over a certain time period
		/// </summary>
		/// <param name="firstPortfolio">First portfolio to be compared</param>
		/// <param name="secondPortfolio">Second portfolio to be compared</param>
		/// <param name="numberOfMonths">Number of months to compare data over (max 60 due to API constraints)</param>
		/// <returns></returns>
		public async Task<PortfolioComparisonDto> ComparePortfolios(Models.Portfolio firstPortfolio, Models.Portfolio secondPortfolio, int numberOfMonths = 60)
		{
			#region Validation
			if (numberOfMonths > 60)
				throw new ArgumentOutOfRangeException("Parameter must be <= 60", "numberOfMonths");

			if (firstPortfolio.Id == secondPortfolio.Id)
				throw new ArgumentException("First and second portfolios are the same");
			#endregion
			PortfolioComparisonDto data = new PortfolioComparisonDto();
			data.NumberOfMonths = numberOfMonths;

			var firstPortfolioReturns = await this.CalculatePortfoliosReturn(firstPortfolio, numberOfMonths);
			var secondPortfolioReturns = await this.CalculatePortfoliosReturn(secondPortfolio, numberOfMonths);

			data.Returns.Add(firstPortfolio.Id, firstPortfolioReturns);
			data.Returns.Add(secondPortfolio.Id, secondPortfolioReturns);
		}

		private async Task<double> CalculatePortfoliosReturn(Models.Portfolio portfolio, int numberOfMonths)
		{
			var stockPortfolios = await _portfolioService.GetStocksFromPortfolio(portfolio);

			double totalPortfolioValue = 0;
			Dictionary<Guid, double> stockTotalValue = new Dictionary<Guid, double>();
			PreviousCloseReqCommand latestInfoCmd = new PreviousCloseReqCommand();
			DailyOpenCloseReqCommand oldInfoCmd = new DailyOpenCloseReqCommand();
			oldInfoCmd.date = DateTime.UtcNow.AddMonths(-1 * numberOfMonths);

			foreach (var stockItem in stockPortfolios)
			{
				latestInfoCmd.stocksTicker = stockItem.Key.Ticker.ToUpper();
				var previousCloseData = await _polygonTickerService.PreviousCloseV2(latestInfoCmd);

				oldInfoCmd.stocksTicker = stockItem.Key.Ticker.ToUpper();
				var oldPriceData = await _polygonTickerService.DailyOpenCloseV1(oldInfoCmd);

				var tupleOfCorrectdata = await GetAvailableOpenClose(oldPriceData, oldInfoCmd);
				oldInfoCmd = tupleOfCorrectdata.Item2;
				oldPriceData = tupleOfCorrectdata.Item1;
				// Have correct info now calculate
			}
		}

		private async Task<Tuple<DailyOpenCloseJto, DailyOpenCloseReqCommand>> GetAvailableOpenClose(DailyOpenCloseJto? data, DailyOpenCloseReqCommand cmd)
		{
			if (data != null && data.status == "NOT_FOUND")
			{
				cmd.date = cmd.date.Value.AddDays(1);
				var newData = await _polygonTickerService.DailyOpenCloseV1(cmd);
				return await GetAvailableOpenClose(data, cmd);
			}
			else if (data != null && data.status == "OK")
			{
				return new Tuple<DailyOpenCloseJto, DailyOpenCloseReqCommand>(data, cmd);
			}
			throw new Exception("data could not be retrieved for some reason LOL");
		}
	}
}
