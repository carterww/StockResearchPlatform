namespace StockResearchPlatform.Services
{
    // Class to hold HttpClient
    // IMPORTANT: Should be a singleton service
    public class HttpService
    {
        private HttpClient _client = new HttpClient();
        public HttpClient Client { get { return _client; } }
        public HttpService()
        {
            
        }
    }
}
