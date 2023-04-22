using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Services;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Models.PageModels
{
    public class PortfolioDisplayModel
    {
        #region Services
        private StockService _stockService;
        private PortfolioService _portfolioService;
        private PolygonTickerService _polygonTickerService;
        #endregion

        #region Props
        public Portfolio Portfolio { get; set; }
        public Dictionary<Stock, StockPortfolio> Stocks { get; set; }
        public Dictionary<Stock, SnapshotsTickerV2Jto> CurrentPricingData { get; set; }
        public Dictionary<Stock, SimpleMovingAverageV1Jto> StockToSimpleMovingAverage { get; set; }
        public bool HideNewStock { get; set; }
        public string AddStockTicker { get; set; }
        public double? AddStockCostBasis { get; set; }
        public double? AddStockNumOfShares { get; set; }
        #endregion

        public PortfolioDisplayModel(StockService stockService, PortfolioService portfolioService, PolygonTickerService polygonTickerService)
        {
            _stockService = stockService;
            _portfolioService = portfolioService;
            _polygonTickerService = polygonTickerService;
        }

        public async Task BuildPortfolioDisplayModel(int id)
        {
            Portfolio tmpPortfolio = _portfolioService.GetPortfolio(id);

            if (tmpPortfolio != null)
            {
                this.Portfolio = tmpPortfolio;
            }
            else
            {
                throw new Exception("Portfolio could not be found.");
            }

            this.Stocks = await _portfolioService.GetStocksFromPortfolio(this.Portfolio);
            this.CurrentPricingData = new Dictionary<Stock, SnapshotsTickerV2Jto>(Stocks.Count);
            this.StockToSimpleMovingAverage = new Dictionary<Stock, SimpleMovingAverageV1Jto>(Stocks.Count);
            foreach (var kv in this.Stocks)
            {
                var pricingtask = _polygonTickerService.SnapshotsTicker(kv.Key.Ticker);
                var smaTask = _polygonTickerService.SimpleMovingAverage(kv.Key.Ticker);
                await Task.WhenAll(pricingtask, smaTask);
                this.CurrentPricingData[kv.Key] = pricingtask.Result;
                this.StockToSimpleMovingAverage[kv.Key] = smaTask.Result;
            }
            this.ClearNewStock();
        }

        public void ShowNewStock()
        {
            this.HideNewStock = false;
        }

        public async Task AddNewStock()
        {
            if (!String.IsNullOrWhiteSpace(this.AddStockTicker)
                && this.AddStockCostBasis != null && this.AddStockCostBasis > 0.0
                && this.AddStockNumOfShares != null && this.AddStockNumOfShares > 0.0)
            {
                this.AddStockTicker = this.AddStockTicker.Trim().ToLower(); // Format ticker

                StockPortfolio tmpSPort = new StockPortfolio();
                Stock tmpS = await _stockService.GetStock(this.AddStockTicker, null);
                if (tmpS == null) throw new Exception("No stock found with that ticker.");

                tmpSPort.FK_Portfolio = this.Portfolio.Id;
                tmpSPort.FK_Stock = tmpS.Id;
                tmpSPort.NumberOfShares = this.AddStockNumOfShares.Value;
                tmpSPort.CostBasis = this.AddStockCostBasis.Value;
                tmpSPort.CreatedOn = DateTime.UtcNow.AddHours(-4); // Eastern Time

                var checkIfExistsList = this.Portfolio.StockPortfolios.Where(s => s.FK_Stock == tmpS.Id);

                // If stock already exists in portfolio
                Console.WriteLine($"HERE: {checkIfExistsList.Count() > 0}");
                if (checkIfExistsList.Count() > 0)
                {
                    var oldShareCount = checkIfExistsList.First().NumberOfShares;
                    double newShareCount = (this.AddStockNumOfShares ?? 0) + oldShareCount;
                    Console.WriteLine($"New share count: {newShareCount}");

                    var oldCostBasis = checkIfExistsList.First().NumberOfShares;
                    double newCostBasis = (((this.AddStockCostBasis ?? 0) * (this.AddStockNumOfShares ?? 0)) + (oldShareCount * oldCostBasis)) / newShareCount;
                    Console.WriteLine($"New cost basis: {newCostBasis}");

                    tmpSPort.CostBasis = newCostBasis;
                    tmpSPort.NumberOfShares = newShareCount;

                    this.Portfolio = this._portfolioService.UpdateStockPortfolio(tmpSPort);
                }
                else
                {
                    this.Portfolio = await this._portfolioService.AddStockToPortfolio(tmpSPort);
                }
                this._portfolioService.SaveChanges();
                await this.BuildPortfolioDisplayModel(this.Portfolio.Id);

                this.ClearNewStock();
            }
        }

        public void ClearNewStock()
        {
            this.HideNewStock = true;
            this.AddStockCostBasis = null;
            this.AddStockNumOfShares = null;
            this.AddStockTicker = "";
        }

        public async void RemoveStock(StockPortfolio stockPortfolio)
        {
            this._portfolioService.RemoveStockPortfolio(stockPortfolio);
            this._portfolioService.SaveChanges();
            this.Portfolio = _portfolioService.GetPortfolio(stockPortfolio.FK_Portfolio);
            await this.BuildPortfolioDisplayModel(this.Portfolio.Id);
        }

        public async void EditStock(StockPortfolio stockPortfolio)
        {
            this.Portfolio = this._portfolioService.UpdateStockPortfolio(stockPortfolio);
            await this.BuildPortfolioDisplayModel(this.Portfolio.Id);
        }
    }
}
