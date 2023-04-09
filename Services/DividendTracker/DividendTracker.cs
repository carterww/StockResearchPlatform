using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
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
        private readonly DividendLedgerRepository _dividendLedgerRepository;

		public DividendTracker
            (
            PolygonDividendService dividendService,
            DividendInfoRepository dividendRepo,
            PortfolioService portService,
            DividendLedgerRepository dividendLedgerRepository
            )
		{
            _dividendService = dividendService;
            _dividendInfoRepository = dividendRepo;
            _portfolioService = portService;
            _dividendLedgerRepository = dividendLedgerRepository;
		}

        public async Task<bool> AddDividendToLedger(List<StockPortfolio> stockPortfolios)
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
                    // Make user a ledger
                    if (currentLedger == null)
                    {
                        var newUserLedger = new DividendLedger();
                        newUserLedger.FK_User = user;

                        _dividendLedgerRepository.Create(newUserLedger);
                        currentLedger = _dividendLedgerRepository.Retrieve(newUserLedger);
                    }

                    var latestDividend = _dividendInfoRepository.Retrieve(currentStockPortfolio.FK_Stock);
                    if (latestDividend == null) continue;

                    var latestLedgerEntry = currentLedger.StockDividendLedgers.Where(e => e.FK_Stock.Id == currentStockPortfolio.FK_Stock).ToList().MaxBy(e => e.Date);

                    // Already added dividend
                    if (latestLedgerEntry != null && latestDividend.PayDate.Day == latestLedgerEntry.Date.Day &&
                        latestDividend.PayDate.Month == latestLedgerEntry.Date.Month &&
                        latestDividend.PayDate.Year == latestLedgerEntry.Date.Year)
                    {
                        continue;
                    }

                    // If stock was added on or after ex dividend date, don't add it
                    if (currentStockPortfolio.CreatedOn.Date >= latestDividend.ExDividendDate.Date) continue;
                    var ledgerentry = new StockDividendLedger();
                    ledgerentry.Date = latestDividend.PayDate;
                    ledgerentry.FK_DividendLedger = currentLedger;
                    ledgerentry.Amount = latestDividend.Cashamount * currentStockPortfolio.NumberOfShares;
                    ledgerentry.FK_Stock = currentStockPortfolio.Stock;

                    // Add entry
                    _dividendLedgerRepository.CreateEntry(ledgerentry);
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

            bool addDividendLedger = await this.AddDividendToLedger(allStocksInUserPortfolios);

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

