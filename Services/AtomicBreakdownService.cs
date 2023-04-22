using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using StockResearchPlatform.Commands;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;
using StockResearchPlatform.Models.PolygonModels;
using StockResearchPlatform.Services.Polygon;
using System.Collections.Concurrent;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace StockResearchPlatform.Services
{
    public class AtomicBreakdownService
    {
        ApplicationDbContext _context;
        public AtomicBreakdownService(ApplicationDbContext context, PolygonTickerService polygonTickerService)
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
            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>();
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
                    dict.TryAdd(FormatString(data.name.ToString()), "100");
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

                            int maxConcurrency = 8;
                            SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);

                            List<Task> tasks = new List<Task>();
                            foreach (XmlNode investment in investments)
                            {
                                tasks.Add(Task.Run(async () =>
                                {
                                    await semaphore.WaitAsync(); // Wait for an available slot
                                    try
                                    {
                                        string name = FormatString(investment["name"].InnerText);

                                        string pctVal = investment["pctVal"].InnerText;

                                        dict.AddOrUpdate(
                                            name,
                                            pctVal,
                                            (key, oldValue) =>
                                            {
                                                double currentValue = double.Parse(oldValue);
                                                double newValue = currentValue + double.Parse(pctVal);
                                                return newValue.ToString();
                                            });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                    finally
                                    {
                                        semaphore.Release(); // Release the slot
                                    }
                                }));
                            }

                            await Task.WhenAll(tasks);

                            break;
                        } 
                        else
                        {
                            index++;
                        }
                    }
                }
            }
            foreach (var item in dict)
            {

            }
            return new Dictionary<string, string>(dict);
        }

        public async Task<Dictionary<string, string>> BreakDownMutualFund(string ticker)
        {

            ConcurrentDictionary<string, string> dict = new ConcurrentDictionary<string, string>();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(300);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");

                MutualFundClass mutualFund = GetMutualFund(ticker);
                string cik = mutualFund.Stock.CIK.ToString();
                cik = cik.PadLeft(10, '0');
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

                        if (GetSeriesId(cik, accNum).Result == seriesId)
                        {
                            Console.WriteLine("Found it!");
                            string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml");
                            

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlString);

                            XmlNodeList investments = doc.GetElementsByTagName("invstOrSec");

                            int maxConcurrency = 8;
                            SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

                            List<Task> tasks = new List<Task>();

                            foreach (XmlNode investment in investments)
                            {
                                await semaphore.WaitAsync();

                                tasks.Add(Task.Run(async () =>
                                {
                                    string name = FormatString(investment["name"].InnerText);
                                    string pctVal = investment["pctVal"].InnerText;

                                    try
                                    {
                                        dict.TryAdd(name, pctVal);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{name}: {pctVal} - {ex.Message}");
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                }));
                            }

                            await Task.WhenAll(tasks);
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
                return new Dictionary<string, string>(dict);
            }
        }

        private static async Task<string> GetSeriesId(string cik, string accNum)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DavidQiu david.qiu179@topper.wku.edu");
                string xmlString = await client.GetStringAsync($"https://www.sec.gov/Archives/edgar/data/{cik}/{accNum}/primary_doc.xml").ConfigureAwait(false);
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
        public static Dictionary<string, string> MultiplyValues(Dictionary<string, string> input, double multiplier)
        {
            Dictionary<string, string> output = new();

            foreach (KeyValuePair<string, string> entry in input)
            {
                double value = double.Parse(entry.Value);
                double result = value * multiplier;
                result /= 100;
                output.Add(entry.Key, result.ToString("F12"));
            }

            return output;
        }

        public static string FormatString(string input)
        {
            // Check if the string ends with "/The" and move it to the start
            if (input.EndsWith("/The", StringComparison.OrdinalIgnoreCase))
            {
                input = "The " + input.Substring(0, input.Length - 4);
            }

            // Replace all '.' with ' '
            input = input.Replace('.', ' ');

            // Split the string into words
            string[] words = input.Split(' ');

            // Capitalize the first letter of each word, except for 'of'
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].ToLower() == "of")
                {
                    words[i] = words[i].ToLower();
                }
                else
                {
                    words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower());
                }
            }

            // Join the words back together
            string formattedString = string.Join(" ", words).Trim();

            return formattedString;
        }

        public static Dictionary<string, double> SortByValueDescending(Dictionary<string, double> dict)
        {
            return dict.OrderByDescending(x => x.Value)
                       .ToDictionary(x => x.Key, x => x.Value);
        }


        public async Task<Dictionary<string, double>> AtomicBreakdown(Dictionary<Stock, StockPortfolio>stocks)
        {
            List<Dictionary<string, string>> listOfDictionaries = new List<Dictionary<string, string>>();
            foreach (var stock in stocks)
            {
                var breakdown = await BreakDownInvestment(stock.Key.Ticker);

                Dictionary<string, string> output = AtomicBreakdownService.MultiplyValues(breakdown, stock.Value.NumberOfShares * stock.Value.CostBasis);
                listOfDictionaries.Add(output);
            }

            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (var dict in listOfDictionaries)
            {
                foreach (var entry in dict)
                {
                    double value = double.Parse(entry.Value);

                    if (result.ContainsKey(entry.Key))
                    {
                        result[entry.Key] += value;
                    }
                    else
                    {
                        result[entry.Key] = value;
                    }
                }
            }
            result = SortByValueDescending(result);
            
            return result;
        }

        public static Dictionary<string, double> ConvertBreakDownToPercentage(Dictionary<string, double> dict)
        {
            double sum = dict.Values.Sum();
            Dictionary<string, double> percentages = new();

            foreach (var entry in dict)
            {
                double percentage = (entry.Value / sum);
                percentages.Add(entry.Key, percentage);
            }

            return percentages;
        }
    }
}
