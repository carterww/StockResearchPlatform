using System.Text;

namespace StockResearchPlatform.Commands
{
	/// <summary>
	/// Enum that holds values that the property "type" can be given.
	/// Use the name of the Enumeration, not the value.
	/// </summary>
	public enum TypeEnum
	{
		CS,
		ADRC,
		ADRP,
		ADRR,
		UNIT,
		RIGHT,
		PFD,
		FUND,
		SP,
		WARRANT,
		INDEX,
		ETF,
		ETN,
		OS,
		GDR,
		OTHER,
		NYRS,
		AGEN,
		EQLK,
		BOND,
		ADRW,
		BASKET,
		LT
	}

	/// <summary>
	/// Enum that holds the values that the property "market" can be given.
	/// Use the name of the Enumeration, not the value.
	/// </summary>
	public enum MarketEnum
	{
		stocks,
		crypto,
		fx,
		otc,
		indices
	}

	public class TickersReqCommand : IReqCommand
	{
		#region SortConstants
		public static readonly string EX_DIVIDEND_DATE_SORT = "ex_dividend_date";
		public static readonly string PAY_DATE_SORT = "pay_date";
		public static readonly string DECLARATION_DATE_SORT = "declaration_date";
		public static readonly string RECORD_DATE_SORT = "record_date";
		public static readonly string CASH_AMOUNT_SORT = "cash_amount";
		public static readonly string TICKER_SORT = "ticker";
		#endregion

		public string ticker { get; set; } = "";
		public TypeEnum? type { get; set; }
		public MarketEnum? market { get; set; }
		public string exchange { get; set; } = "";
		public string cusip { get; set; } = "";
		public string cik { get; set; } = "";
		public string date { get; set; } = "";
		public string search { get; set; } = "";
		public bool active { get; set; } = true;

		private string? _order = "";
		public string? order
		{
			get => _order;
			set
			{
				if (value == StockFinancialsReqCommand.ASC_ORDER ||
					value == StockFinancialsReqCommand.DESC_ORDER)
				{
					_order = value;
				}
				else throw new ArgumentOutOfRangeException($"order must be \'{StockFinancialsReqCommand.ASC_ORDER}\' or \'{StockFinancialsReqCommand.DESC_ORDER}\'.");
			}
		}

		private int _limit = 1;
		public int limit
		{
			get => _limit;
			set
			{
				if (value >= StockFinancialsReqCommand.MIN_LIMIT &&
					value <= StockFinancialsReqCommand.MAX_LIMIT)
				{
					_limit = value;
				}
				else throw new ArgumentOutOfRangeException($"limit must be between {StockFinancialsReqCommand.MIN_LIMIT} and {StockFinancialsReqCommand.MAX_LIMIT}");
			}
		}
		private string? _sort = "";
		public string? sort
		{
			get => _sort;
			set
			{
				if (value == EX_DIVIDEND_DATE_SORT ||
					value == PAY_DATE_SORT ||
					value == DECLARATION_DATE_SORT ||
					value == RECORD_DATE_SORT ||
					value == CASH_AMOUNT_SORT ||
					value == TICKER_SORT)
				{
					_sort = value;
				}
				else throw new ArgumentOutOfRangeException($"sort must be one of the static readonly strings in the DividendsReqCommand class");
			}
		}

		/// <summary>
		/// Returns a string URL that contains all the parameters specified by the class. Leaves out all parameters not specified
		/// </summary>
		/// <param name="baseUrl">Polygon's base URL</param>
		/// <param name="endpoint">Endpoint URL</param>
		/// <param name="apiKey">Polygon API key</param>
		/// <returns>A string URL with all query params specified</returns>
		public string BuildQueryParams(string baseUrl, string endpoint, string apiKey)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(baseUrl);
			sb.Append(endpoint);
			sb.Append($"?apiKey={apiKey}");

			#region AddParams

			if (!String.IsNullOrEmpty(this.ticker)) sb.Append($"&ticker={this.ticker}");
			if (type != null) sb.Append($"&type={type.ToString()}");
			if (market != null) sb.Append($"&market={market.ToString()}");
			if (!String.IsNullOrEmpty(this.exchange)) sb.Append($"&exchange={exchange}");
			if (!String.IsNullOrEmpty(cusip)) sb.Append($"&cusip={cusip}");
			if (!String.IsNullOrEmpty(cik)) sb.Append($"&cik={cik}");
			if (!String.IsNullOrEmpty(date)) sb.Append($"&date={date}");
			if (!String.IsNullOrEmpty(search)) sb.Append($"&search={search}");
			sb.Append($"&active={active}");
			if (!String.IsNullOrEmpty(this.order)) sb.Append($"&order={this.order}");
			sb.Append($"&limit={this.limit}");
			if (!String.IsNullOrEmpty(sort)) sb.Append($"&sort={this.sort}");

			#endregion

			return sb.ToString();
		}
	}

}
