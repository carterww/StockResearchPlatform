using System.Text;

namespace StockResearchPlatform.Commands
{
	/// <summary>
	/// Enum that holds values that the property "frequency" can be given.
	/// Use the value of the Enumeration, not the name.
	/// </summary>
	public enum FrequencyEnum
	{
		ZERO = 0,
		ONE = 1,
		TWO = 2,
		FOUR = 4,
		TWELVE = 12
	}

	/// <summary>
	/// Enum that holds the values that the property "dividend_type" can be given.
	/// Use the name of the Enumeration, not the value.
	/// </summary>
	public enum DividendTypeEnum
	{
		CD,
		SC,
		LT,
		ST
	}

	public class DividendsReqCommand : IReqCommand
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
		public PolygonParamInequality? ex_dividend_date { get; set; }
		public PolygonParamInequality? record_date { get; set; }
		public PolygonParamInequality? declaration_date { get; set; }
		public PolygonParamInequality? pay_date { get; set; }
		public FrequencyEnum? frequency { get; set; }
		public PolygonParamInequality? cash_amount { get; set; }
		public DividendTypeEnum? dividend_type { get; set; }

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

			if (!String.IsNullOrEmpty(this.ticker))			sb.Append($"&ticker={this.ticker}");
			if (ex_dividend_date != null)					sb.Append($"&{ex_dividend_date.nameof}={ex_dividend_date.value}");
			if (record_date != null)						sb.Append($"&{record_date.nameof}={record_date.value}");
			if (declaration_date != null)					sb.Append($"&{declaration_date.nameof}={declaration_date.value}");
			if (pay_date != null)							sb.Append($"&{pay_date.nameof}={pay_date.value}");
			if (frequency != null)							sb.Append($"&frequency={(int)frequency}");
			if (cash_amount != null)						sb.Append($"&{cash_amount.nameof}={cash_amount.value}");
			if (dividend_type != null)						sb.Append($"&dividend_type={dividend_type.ToString()}");
			if (!String.IsNullOrEmpty(this.order))			sb.Append($"&order={this.order}");
															sb.Append($"&limit={this.limit}");
			if (!String.IsNullOrEmpty(sort))				sb.Append($"&sort={this.sort}");

			#endregion

			return sb.ToString();
		}
	}
}
