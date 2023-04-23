﻿using StockResearchPlatform.Commands;
using StockResearchPlatform.Models;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Models.PortfolioDtos;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Services.PortfolioComparison
{
	// Alias for static class used for helping with calculations
	using Utils = PortfolioComparisonUtils;
	public class PortfolioComparisonService
	{
		public const double RISK_FREE_RATE_OF_RETURN = 0.036;

		private readonly PolygonTickerService _polygonTickerService;
		private readonly PortfolioService _portfolioService;

		public double[]? BasePriceHistory { get; set; } = null;
		public double[]? BasePriceHistoryDailyChanges { get; set; } = null;

		public PortfolioComparisonService (
			PolygonTickerService polygonTickerService,
			PortfolioService portfolioService
			) 
		{ 
			_polygonTickerService = polygonTickerService;
			_portfolioService = portfolioService;
		}

		/// <summary>
		/// I made a previous version of this, but it lacked more data
		/// </summary>
		/// <param name="firstPortfolio"></param>
		/// <param name="secondPortfolio"></param>
		/// <param name="numberOfMonths"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<PortfolioComparisonDto> ComparePortfoliosV2(Models.Portfolio firstPortfolio, Models.Portfolio secondPortfolio, int numberOfMonths = 60, string baseTicker = "SPY")
		{
			#region Validation
			var isValidTuple = VerifyParametersForComparison(firstPortfolio, secondPortfolio, numberOfMonths);
			if (isValidTuple.Item1 == false)
			{
				throw new Exception(isValidTuple.Item2);
			}
			#endregion

			var aggregatesReqCommand = new AggregateBarsReqCommand();
			aggregatesReqCommand.stocksTicker = baseTicker;
			aggregatesReqCommand.from = DateTime.UtcNow.AddMonths(-1 * numberOfMonths);
			aggregatesReqCommand.to = DateTime.UtcNow;

			var spyPriceHistoryJto = await _polygonTickerService.AggregatesBarsV2(aggregatesReqCommand);

			if (spyPriceHistoryJto == null)
				throw new Exception("Cannot do this  at this time");

			// Put prices in array for faster access
			this.BasePriceHistory = this.CovertAggregateJtoToArrayOfPrices(spyPriceHistoryJto);
			this.BasePriceHistoryDailyChanges = this.GetPriceHistoryChanges(this.BasePriceHistory);

			Task<PortfolioComparisonHelperDto>[] work = new Task<PortfolioComparisonHelperDto>[2];
			work[0] = this.GetPortfolioInfo(firstPortfolio, numberOfMonths, aggregatesReqCommand);

			// Create deep copy so concurrency does not cause issues by sharing same object
			var aggregatesReqCommand2 = new AggregateBarsReqCommand();
			aggregatesReqCommand2.stocksTicker = "";
			aggregatesReqCommand2.from = DateTime.UtcNow.AddMonths(-1 * numberOfMonths);
			aggregatesReqCommand2.to = DateTime.UtcNow;
			work[1] = this.GetPortfolioInfo(secondPortfolio, numberOfMonths, aggregatesReqCommand2);

			PortfolioComparisonHelperDto[] results = await Task.WhenAll(work);

			var firstPortfolioComparisonHelper = results[0];
			var secondPortfolioComparisonHelper = results[1];

			PortfolioComparisonDto result = new PortfolioComparisonDto();
			result.NumberOfMonths = numberOfMonths;
			result.Values.Add(firstPortfolio.Id, firstPortfolioComparisonHelper);
			result.Values.Add(secondPortfolio.Id, secondPortfolioComparisonHelper);

			return result;
		}

		private async Task<PortfolioComparisonHelperDto> GetPortfolioInfo(Models.Portfolio portfolio, int numberOfMonths, AggregateBarsReqCommand cmd)
		{
			var stockStockPortfolios = await _portfolioService.GetStocksFromPortfolio(portfolio);

			var data = new PortfolioComparisonHelperDto();
			data.PortfolioId = portfolio.Id;
			data.PortfolioPriceHistory = new double[this.BasePriceHistory.Length];

			foreach (var stockStockPortfolio in stockStockPortfolios)
			{
				// Get total portfolio price in array
				cmd.stocksTicker = stockStockPortfolio.Key.Ticker.ToUpper();
				var stockJto = await _polygonTickerService.AggregatesBarsV2(cmd);
				if (stockJto != null && stockJto.resultsCount == this.BasePriceHistory.Length)
				{
					var numOfSharesTmp = stockStockPortfolio.Value.NumberOfShares;
					for (int i = 0; i < this.BasePriceHistory.Length; i++)
					{
						data.PortfolioPriceHistory[i] += (stockJto.results[i].c * numOfSharesTmp);
					}
				}
			}

			data.PortfolioPriceHistoryChange = this.GetPriceHistoryChanges(data.PortfolioPriceHistory);

			data.Beta = Utils.CalculateBeta(this.BasePriceHistoryDailyChanges, data.PortfolioPriceHistoryChange);
			data.TotalReturn = Utils.CalculateReturn(data.PortfolioPriceHistory[0], data.PortfolioPriceHistory[data.PortfolioPriceHistory.Length - 1], 1);

			var marketReturn = Utils.CalculateReturn(this.BasePriceHistory[0], this.BasePriceHistory[this.BasePriceHistory.Length - 1], 1);
			data.JensensAlpha = Utils.CalculateJensensAplha(data.TotalReturn, RISK_FREE_RATE_OF_RETURN, data.Beta, marketReturn);
			data.Alpha = Utils.CalculateAlpha(data.TotalReturn, marketReturn);
			data.PortfolioName = portfolio.Name;

			return data;
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
			var isValidTuple = VerifyParametersForComparison(firstPortfolio, secondPortfolio, numberOfMonths);
			if (isValidTuple.Item1 == false)
			{
				throw new Exception(isValidTuple.Item2);
			}
			#endregion
			PortfolioComparisonDto data = new PortfolioComparisonDto();
			data.NumberOfMonths = numberOfMonths;

			// For now, just calcualte total returns
			// Expand to alpha and beta if we have time
			var firstPortfolioReturns = await this.CalculatePortfoliosReturn(firstPortfolio, numberOfMonths);
			var secondPortfolioReturns = await this.CalculatePortfoliosReturn(secondPortfolio, numberOfMonths);

			data.Returns.Add(firstPortfolio.Id, firstPortfolioReturns);
			data.Returns.Add(secondPortfolio.Id, secondPortfolioReturns);

			return data;
		}

		private Tuple<bool, string> VerifyParametersForComparison(Models.Portfolio firstPortfolio, Models.Portfolio secondPortfolio, int numberOfMonths)
		{
			var canCompare = new Tuple<bool, string>(true, string.Empty);
			if (numberOfMonths > 60)
				canCompare = new Tuple<bool, string>(false, "NumberOfMonths must be <= 60");

			if (firstPortfolio.Id == secondPortfolio.Id)
				canCompare = new Tuple<bool, string>(false, "First and second portfolios are the same");

			return canCompare;
		}

		private double[] CovertAggregateJtoToArrayOfPrices(AggregatesBarsV2Jto jto)
		{
			var response = new double[jto.resultsCount];

			for (var i = 0; i < response.Length; i++)
			{
				response[i] = jto.results[i].c; // Use close price
			}

			return response;
		}

		private double[] GetPriceHistoryChanges(double[] prices)
		{
			var response = new double[prices.Length];

			for (int i = 1; i < prices.Length; i++)
			{
				response[i] = (prices[i] - prices[i - 1]) / prices[i - 1]; // Daily percent change compared to previous day
			}

			return response;
		}

		private double[] GetPriceHistoryChanges(AggregatesBarsV2Jto jto)
		{
			var response = new double[jto.resultsCount];

			for (int i = 1; i < jto.resultsCount; i++)
			{
				response[i] = (jto.results[i].c - jto.results[i - 1].c) / jto.results[i - 1].c; // Daily percent change compared to previous day
			}

			return response;
		}

		private double AverageDailyChange(double[] changes, int numOfDataPoints)
		{
			double total = 0;

			for (int i = 1; i < changes.Length; i++)
			{
				total += changes[i];
			}

			return total / numOfDataPoints;
		}

		/// <summary>
		/// Helper function to ComparePortfolios that calculates a portfolio's return over the given time period
		/// </summary>
		/// <returns>Returns a double that represents the percentage gain over the time period</returns>
		private async Task<double> CalculatePortfoliosReturn(Models.Portfolio portfolio, int numberOfMonths)
		{
			var stockPortfolios = await _portfolioService.GetStocksFromPortfolio(portfolio);

			double totalPortfolioValue = 0;
			// Holds total value as Item1 and total gain amount as Item2
			Dictionary<Guid, Tuple<double, double>> stockTotalValueAndReturns = new Dictionary<Guid, Tuple<double, double>>();

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
				if (oldPriceData != null && previousCloseData != null && previousCloseData.resultsCount > 0)
				{
					var totalStockValue = stockItem.Value.CostBasis * stockItem.Value.NumberOfShares;
					stockTotalValueAndReturns.Add(stockItem.Key.Id, new Tuple<double, double>(totalStockValue, Utils.CalculateReturn(oldPriceData.close, previousCloseData.results[0].c, stockItem.Value.NumberOfShares)));
					totalPortfolioValue += totalStockValue;
				}
			}

			double weightedTotalReturn = 0;
			foreach (var stockReturn in stockTotalValueAndReturns)
			{
				var totalStockValue = stockReturn.Value.Item1;
				var gainValue = stockReturn.Value.Item2;

				weightedTotalReturn += (totalStockValue / totalPortfolioValue) * gainValue;
			}

			return weightedTotalReturn;
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