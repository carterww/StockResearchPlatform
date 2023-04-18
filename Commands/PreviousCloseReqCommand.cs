using System.Text;

namespace StockResearchPlatform.Commands
{
	public class PreviousCloseReqCommand : IReqCommand
	{
		public string stocksTicker { get; set; } = "";
		public bool adjusted { get; set; } = true;

		virtual public string BuildQueryParams(string baseUrl, string endpoint, string apiKey)
		{
			StringBuilder sb = new StringBuilder();
			if (!String.IsNullOrEmpty(this.stocksTicker))
			{
				endpoint = string.Format(endpoint, stocksTicker.ToUpper());
			}
			sb.Append(baseUrl);
			sb.Append(endpoint);
			sb.Append($"?apiKey={apiKey}");

			sb.Append($"&adjusted={adjusted}");

			return sb.ToString();
		}
	}
}
