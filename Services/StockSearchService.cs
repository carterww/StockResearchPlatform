using Microsoft.Extensions.Configuration;
using StockResearchPlatform.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockResearchPlatform.Services
{
    public class StockSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        private readonly string? _apiKey;
        private readonly string? _baseUrl;
        
        public StockSearchService(IConfiguration configuration, HttpService httpservice)
        {
            _configuration = configuration;
            _httpService = httpservice;
            _apiKey = _configuration.GetSection("AletheiaAPI")["Key"];
            _baseUrl = _configuration.GetSection("AletheiaAPI")["BaseUrl"];
        }

        public async Task<StockInformation> fetchStockInformation(string ticker)
        {
            var client = _httpService.Client;
            var reqUrl = _baseUrl + $"StockData?symbol={ticker.ToLower()}";

            try
            {
                HttpResponseMessage? res = null;
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, reqUrl))
                {
                    requestMessage.Headers.Add("Key", _apiKey);
                    requestMessage.Headers.Add("Accept-Version", "2");

                    res = await client.SendAsync(requestMessage);
                }

                if (res != null)
                {
                    res.EnsureSuccessStatusCode();
                    string resBody = await res.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<StockInformation>(resBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
}
