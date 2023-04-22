namespace StockResearchPlatform.Services
{
    // Class to hold HttpClient
    // IMPORTANT: Should be a singleton service
    public class HttpService
    {
        private HttpClient _client;
        public HttpClient Client { get { return _client; } }
        private HttpClientHandler _handler = new HttpClientHandler();
        public HttpClientHandler Handler { get { return _handler; } }
        public HttpService()
        {
            _client = new HttpClient(Handler, false);
        }
    }
}
