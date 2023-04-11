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

        public Stock GetStock(string ticker)
        {
            var stock = _context.Stocks.Where(s => s.Ticker == ticker).First();
            if (stock != null)
            {
                return stock;

            }
            return null;
        }

        public async Task<Dictionary<string, string>> BreakDownMutualFund(string ticker)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");
                MutualFundClass mutualFund = GetMutualFund(ticker);
                string cik = mutualFund.Stock.CIK.ToString();
                cik = cik.PadLeft(10, '0');
                string seriesId = mutualFund.SeriesID;
                Console.WriteLine(client);
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
                            Console.WriteLine(accNum);
                            string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml");
                            

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlString);

                            XmlNodeList investments = doc.GetElementsByTagName("invstOrSec");
                            Dictionary<string, string> dict = new Dictionary<string, string>();
                            foreach (XmlNode investment in investments)
                            {
                                string name = investment["name"].InnerText;
                                string pctVal = investment["pctVal"].InnerText;
                                try
                                {
                                    dict.Add(name, pctVal);
                                }
                                catch { }
                            }
                            Console.WriteLine(dict);
                            break;
                        }
                        else
                        {
                            index++;
                        }
                        
                    }
                    else
                    {
                        index++;
                    }

                }
                return null;
            }
        }

        private static async Task<string> GetSeriesId(string cik, string accNum, HttpClient client)
        {
            /*client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");*/
            string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml");
            string seriesId = "";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNodeList seriesClass = doc.GetElementsByTagName("seriesClassInfo");

            foreach (XmlNode classInfo in seriesClass)
            {
                seriesId = classInfo.InnerText.Substring(0, classInfo.InnerText.IndexOf('C'));

                Console.WriteLine(seriesId);
            }

            return seriesId;
        }



    }
}
