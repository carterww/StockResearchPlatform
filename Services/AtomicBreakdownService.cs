using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using StockResearchPlatform.Commands;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Services.Polygon;
using System.Xml;

namespace StockResearchPlatform.Services
{
    public class AtomicBreakdownService
    {
        ApplicationDbContext _context;
        private readonly PolygonTickerService _polygonTickerService;
        public AtomicBreakdownService(ApplicationDbContext context, PolygonTickerService polygonTickerService)
        {
            _context = context;
            _polygonTickerService = polygonTickerService;
        }
        // use polygon service to get the data
        public async Task<TickerV3Jto?> GetTickersV3Async(TickersReqCommand queryParams)
        {
            var tickerV3Jto = await _polygonTickerService.TickersV3(queryParams);
            return tickerV3Jto;
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

        public bool CheckedIfStockIsMutualFund(Stock stock)
        {
            if (stock.MutualFund == null)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }

        public bool StockHasMutualFund(string ticker)
        {
            {
                return _context.Stocks.Any(s => s.Ticker == ticker && s.MutualFund != null);
            }
        }

        public async Task<Dictionary<string, string>> BreakDownInvestment(string ticker)
        {
            
            if (StockHasMutualFund(ticker))
            {
                return await BreakDownMutualFund(ticker);
            }
            else
            {
                return await BreakDownStock(ticker);
            }
        }

        public async Task<string> convertName(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                var formattedName = name.Replace(" ", "%20");
                string json = await client.GetStringAsync($"https://api.polygon.io/v3/reference/tickers?search={formattedName}/The&active=true&apiKey=y0U2aJL0Js5cP0PyR0Uf4D1mc2E0fb0A");
                dynamic data = JsonConvert.DeserializeObject(json);
                return data.results[0].name;
            }
        }
        public async Task<Dictionary<string, string>> BreakDownStock(string ticker)
        {
            Console.WriteLine(await convertName("Boeing Co/The"));
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(300);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");
                Stock stock = GetStock(ticker);
                string cik = stock.CIK.ToString();
                cik = cik.PadLeft(10, '0');
                string json = await client.GetStringAsync($"https://data.sec.gov/submissions/CIK{cik}.json");
                dynamic data = JsonConvert.DeserializeObject(json);

                if (data.entityType == "operating")
                {
                    Console.WriteLine("test");
                } 
                else
                {
                    int index = 0;

                    foreach (string filingForm in data.filings.recent.form)
                    {
                        if (filingForm == "NPORT-P")
                        {
                            string accNum = data.filings.recent.accessionNumber[index];
                            accNum = accNum.Replace("-", "");

                            string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml");

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlString);

                            XmlNodeList investments = doc.GetElementsByTagName("invstOrSec");
                            int maxConcurrency = 7; // Set the maximum number of concurrent operations
                            SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);

                            List<Task> tasks = new List<Task>();
                            foreach (XmlNode investment in investments)
                            {
                                tasks.Add(Task.Run(async () =>
                                {
                                    await semaphore.WaitAsync(); // Wait for an available slot
                                    try
                                    {
                                        string name = investment["name"].InnerText;
                                        name = await convertName(name);
                                        string pctVal = investment["pctVal"].InnerText;
                                        try
                                        {
                                            if (dict.ContainsKey(name))
                                            {
                                                double currentValue = double.Parse(dict[name]);
                                                double newValue = currentValue + double.Parse(pctVal);
                                                dict[name] = newValue.ToString();
                                            }
                                            else
                                            {
                                                dict.Add(name, pctVal);
                                            }
                                        }
                                        catch { Console.WriteLine(name + ": " + pctVal); }
                                    }
                                    finally
                                    {
                                        semaphore.Release(); // Release the slot
                                    }
                                }));
                            }
                            await Task.WhenAll(tasks);

                            foreach (KeyValuePair<string, string> kvp in dict)
                            {
                                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                            }
                            break;
                        } 
                        else
                        {
                            index++;
                        }
                    }
                }
                /*foreach (string formType in data.entityType)
                {
                    if (filingForm == "NPORT-P")
                    {

                    }
                }*/
            }
            return dict;
        }

        public async Task<Dictionary<string, string>> BreakDownMutualFund(string ticker)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
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

                            foreach (XmlNode investment in investments)
                            {
                                string name = investment["name"].InnerText;
                                string pctVal = investment["pctVal"].InnerText;
                                try
                                {
                                    dict.Add(name, pctVal);
                                }
                                catch { Console.WriteLine(name + ": " + pctVal); }
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
                return dict;
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
