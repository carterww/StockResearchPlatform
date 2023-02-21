using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Models;
using System.Text.RegularExpressions;

namespace StockResearchPlatform.Data
{
    public class LoadStockDataToDatabaseService
    {
        private const string STOCK_TICKERS_TXT = "tickers-cik.txt";
        private const string MUTUAL_FUNDS_TICKERS_TXT = "mutual-fund-tickers.csv";
        public enum MutualFundColumn: int {
            REPORTING_FILE_NUMBER = 0,
            CIK_NUMBER,
            ENTITY_NAME,
            ENTITY_ORG_TYPE,
            SERIES_ID,
            SERIES_NAME,
            CLASS_ID,
            CLASS_NAME,
            CLASS_TICKER,
            ADDRESS_1,
            ADDRESS_2,
            CITY,
            STATE,
            ZIP_CODE
        };
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

        public void LoadMutualFundsToDatabase()
        {
            StreamReader file = new StreamReader($"Resources/{MUTUAL_FUNDS_TICKERS_TXT}");
            if (file == null) throw new Exception("File not found");
            string ln;
            file.ReadLine();
            Regex regex = new Regex(",(?=(?:(?:[^\"]*\"){2})*[^\"]*$)");

            while ((ln = file.ReadLine()) != null)
            {
                string[] row = regex.Split(ln);

				if (row[(int)MutualFundColumn.CLASS_TICKER] == "[NULL]") continue;

				ulong cik = (ulong)Convert.ToDouble(row[(int)MutualFundColumn.CIK_NUMBER]);
				Stock s = new Stock(row[(int)MutualFundColumn.CLASS_TICKER]);
                s.CIK = cik;
                MutualFundClass m = new MutualFundClass(s.Id);
                m.Stock = s;
                m.SeriesID = row[(int)MutualFundColumn.SERIES_ID];
                m.ClassID = row[(int)MutualFundColumn.CLASS_ID];
                try
                {
                    _dbContext.MutualFunds.Add(m);
                }
                catch (Exception ex)
                {
                    _dbContext.MutualFunds.Remove(m);
                }
            }
			_dbContext.SaveChanges();
			file.Close();
        }
    }
}
