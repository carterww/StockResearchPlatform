using StockResearchPlatform.Models;

namespace StockResearchPlatform.Data
{
    public class LoadStockDataToDatabaseService
    {
        private const string STOCK_TICKERS_TXT = "tickers-cik.txt";
        private readonly ApplicationDbContext _dbContext;

        public LoadStockDataToDatabaseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void LoadStocksToDatabase()
        {
            StreamReader file = new StreamReader($"Resources/{STOCK_TICKERS_TXT}");
            if (file == null) throw new Exception("File not found");
            string ln;

            while ((ln = file.ReadLine()) != null)
            {
                string[] t = ln.Split('\t');
                string ticker = t[0];
                ulong cik = (ulong)Convert.ToDouble(t[1]);
				Stock s = new Stock(ticker);
                s.CIK = cik;
                _dbContext.Stocks.Add(s);
            }
			_dbContext.SaveChanges();
			file.Close();
        }
    }
}
