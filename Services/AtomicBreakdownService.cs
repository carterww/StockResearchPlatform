using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using System.Xml;

namespace StockResearchPlatform.Services
{
    public class AtomicBreakdownService
    {
        ApplicationDbContext _context;

        public AtomicBreakdownService(ApplicationDbContext context)
        {
            _context = context;
        }

        public MutualFundClass GetMutualFund(string ticker)
        {
            var stock = _context.Stocks.Where(s => s.Ticker == ticker).First();
            if (stock != null)
            {
                var mutualFund = _context.MutualFunds.Where(m => m.Stock.Id == stock.Id).First();
                if (mutualFund != null)
                {
                    return mutualFund;
                }
            }
            return null;
        }

        public async Dictionary<string, string> BreakDownMutualFund(string ticker)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");
                MutualFundClass mutualFund = GetMutualFund(ticker);
                string cik = mutualFund.Stock.CIK.ToString();
                string seriesId = mutualFund.SeriesID;
                string json = await client.GetStringAsync($"https://data.sec.gov/submissions/CIK{cik}.json");
                dynamic data = JsonConvert.DeserializeObject(json);

                int index = 0;


                foreach (string filingForm in data.filings.recent.form)
                {
                    if (filingForm == "NPORT-P")
                    {
                        string accNum = data.filings.recent.accessionNumber[index];
                        accNum = accNum.Replace("-", "");

                        if (GetSeriesId(cik, accNum, client).Result == seriesId)
                        {

                        }
                        break;
                    }
                    else
                    {
                        index++;
                    }

                }
            }
        }

        private static async Task<string> GetSeriesId(string cik, string accNum, HttpClient client)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");
            string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNodeList seriesClass = doc.GetElementsByTagName("seriesClassInfo");

            foreach (XmlNode classInfo in seriesClass)
            {
                string seriesId = classInfo.InnerText.Substring(0, classInfo.InnerText.IndexOf('C'));

                Console.WriteLine(seriesId);
            }

            return xmlString;
        }

        private static async Task<string> 
    }
}
