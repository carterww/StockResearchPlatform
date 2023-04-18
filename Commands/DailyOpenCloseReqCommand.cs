using System.Text;

namespace StockResearchPlatform.Commands
{
	public class DailyOpenCloseReqCommand : PreviousCloseReqCommand
	{
		public DateTime? date { get; set; } = null;

		public override string BuildQueryParams(string baseUrl, string endpoint, string apiKey)
		{
			StringBuilder sb = new StringBuilder();
			if (!String.IsNullOrEmpty(this.stocksTicker) && date != null)
			{
				endpoint = string.Format(endpoint, stocksTicker.ToUpper(), string.Format("{0:yyyy-MM-dd}", date));
			}
			sb.Append(baseUrl);
			sb.Append(endpoint);
			sb.Append($"?apiKey={apiKey}");

			sb.Append($"&adjusted={adjusted}");

			return sb.ToString();
		}
	}
}
