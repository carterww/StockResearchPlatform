namespace StockResearchPlatform.Commands
{
	public interface IReqCommand
	{
		public string BuildQueryParams(string baseUrl, string endpoint, string apiKey);
	}
}
