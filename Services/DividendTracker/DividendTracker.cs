using System;
using System.Globalization;
using Hangfire;
using StockResearchPlatform.Commands;
using StockResearchPlatform.Models;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Repositories;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Services.DividendTracker
{
	public class DividendTracker : IDividendTracker
	{
        private readonly PolygonDividendService _dividendService;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly DividendInfoRepository _dividendInfoRepository;
        private readonly PortfolioService _portfolioService;

		public DividendTracker
            (
            PolygonDividendService dividendService,
            DividendInfoRepository dividendRepo,
            PortfolioService portService
            )
		{
            _dividendService = dividendService;
            _dividendInfoRepository = dividendRepo;
            _portfolioService = portService;
		}

        public void AddDividendToLegder()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDividendInfoRecords()
        {
            List<StockPortfolio> allStocksInUserPortfolios = _portfolioService.GetStockPortfolios();

            if (allStocksInUserPortfolios.Count <= 0) return false;
            // Keep track of stocks already checked so we do not repeat mutliple
            HashSet<string> previouslyRetrievedTickers = new HashSet<string>();

            // Command to build query parameters for dividend service
            DividendsReqCommand cmd = new DividendsReqCommand();
            cmd.order = StockFinancialsReqCommand.DESC_ORDER;
            cmd.sort = DividendsReqCommand.EX_DIVIDEND_DATE_SORT;

            for (int i = 0; i < allStocksInUserPortfolios.Count; i++)
            {
                var currentStock = allStocksInUserPortfolios[i].Stock;
                if (currentStock == null) continue;
                cmd.ticker = currentStock.Ticker.ToUpper();
                if (previouslyRetrievedTickers.Contains(cmd.ticker)) continue;

                var dividendJto = await _dividendService.DividendsV3(cmd);
                if (dividendJto != null && MostCurrentDividendInfoExists(dividendJto) == false && dividendJto.status == "OK")
                {
                    DividendInfo infoToAdd = new DividendInfo();
                    infoToAdd.FK_Stock = currentStock.Id;
                    infoToAdd.UpdatedOn = DateTime.Now;
                    infoToAdd.RecordDate = _dividendService.ParsePolygonDate(dividendJto.results[0].record_date);
                    infoToAdd.ExDividendDate = _dividendService.ParsePolygonDate(dividendJto.results[0].ex_dividend_date);
                    infoToAdd.DeclarationDate = _dividendService.ParsePolygonDate(dividendJto.results[0].declaration_date);
                    infoToAdd.PayDate = _dividendService.ParsePolygonDate(dividendJto.results[0].pay_date);
                    infoToAdd.Cashamount = dividendJto.results[0].cash_amount;

                    _dividendInfoRepository.AddOrUpdate(infoToAdd);
				}

                previouslyRetrievedTickers.Add(cmd.ticker);
            }

            throw new NotImplementedException();
        }

        private bool MostCurrentDividendInfoExists(DividendsV3Jto dividendJto)
        {
            var dividendJtoExDate = _dividendService.ParsePolygonDate(dividendJto.results[0].ex_dividend_date);

			var list = _dividendInfoRepository.Retrieve((d) => d.ExDividendDate.Year == dividendJtoExDate.Year && d.ExDividendDate.Month == dividendJtoExDate.Month && d.ExDividendDate.Day == dividendJtoExDate.Day);
            return list.Count > 0;
        }
    }
}

