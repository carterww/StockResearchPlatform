using System;
using System.Globalization;
using System.Runtime.InteropServices;
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

        public async Task<bool> AddDividendToLegder(List<StockPortfolio> stockPortfolios)
        {
			if (stockPortfolios.Count <= 0) return false;

            Dictionary<string, DividendLedger> ledgers = new Dictionary<string, DividendLedger>();

            // Check each stock in all user's portfolios to see if stock paid out dividend
            for (int i = 0; i < stockPortfolios.Count; i++)
            {
                var currentStockPortfolio = stockPortfolios[i];

                if (currentStockPortfolio == null) continue;

                var userId = currentStockPortfolio.Portfolio.FK_UserId;
                var currentLedger = ledgers[userId];
                if (currentLedger == null)
                {
                    var user = _dividendInfoRepository.GetUser(userId);
                    currentLedger = user.DividendLedgers.FirstOrDefault();
                    // TODO Create dividend ledger for user if none there
                }
            }

            return true;
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

            bool addDividendLedger = await this.AddDividendToLegder(allStocksInUserPortfolios);

            return true && addDividendLedger;
        }

        private bool MostCurrentDividendInfoExists(DividendsV3Jto dividendJto)
        {
            var dividendJtoExDate = _dividendService.ParsePolygonDate(dividendJto.results[0].ex_dividend_date);

			var list = _dividendInfoRepository.Retrieve((d) => d.ExDividendDate.Year == dividendJtoExDate.Year && d.ExDividendDate.Month == dividendJtoExDate.Month && d.ExDividendDate.Day == dividendJtoExDate.Day);
            return list.Count > 0;
        }

        private bool DividendLedgerContainsEntry(StockDividendLedger ledgerEntry)
        {
            var list = _dividendInfoRepository.Retrieve((d) => d.PayDate.Year == ledgerEntry.Date.Year &&
                d.PayDate.Month == ledgerEntry.Date.Month && d.PayDate.Day == ledgerEntry.Date.Day);

            return list.Count > 0;
        }
    }
}

