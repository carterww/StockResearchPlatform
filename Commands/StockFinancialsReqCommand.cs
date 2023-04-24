using System.Text;

namespace StockResearchPlatform.Commands
{
	public class StockFinancialsReqCommand : IReqCommand
	{
		#region Timeframestatic readonlyants
		public static readonly string ANNUAL_TIMEFRAME = "annual";
		public static readonly string QUARTERLY_TIMEFRAME = "quarterly";
		public static readonly string TTM_TIMEFRAME = "ttm";
		#endregion

		#region OrderConstants
		public static readonly string ASC_ORDER = "asc";
		public static readonly string DESC_ORDER = "desc";
		#endregion

		#region LimitConstants
		public static readonly int MIN_LIMIT = 1;
		public static readonly int MAX_LIMIT = 100;
		public static readonly int DEFAULT_LIMIT = 10;
		#endregion

		#region SortConstants
		public static readonly string FILING_DATE_SORT = "filing_date";
		public static readonly string PERIOD_OF_REPORT_DATE_SORT = "period_of_report_date";
		#endregion

		public string ticker { get; set; } = "";
		public string? cik { get; set; } = "";
		public string? company_name { get; set; } = "";
		public PolygonParamInequality? filing_date { get; set; }
		public PolygonParamInequality? period_of_report_date { get; set; }
		private string? _timeframe = "annual";
		public string? timeframe { get => _timeframe;
			set
			{
				if (value == ANNUAL_TIMEFRAME ||
					value == QUARTERLY_TIMEFRAME ||
					value == TTM_TIMEFRAME)
				{
					_timeframe = value;
				}
				else throw new ArgumentOutOfRangeException($"timeframe must be \'{ANNUAL_TIMEFRAME}\', \'{QUARTERLY_TIMEFRAME}\', or \'{TTM_TIMEFRAME}\'.");
			}
		}

		public bool? include_sources { get; set; } = false;

		private string? _order = "";
		public string? order { get => _order;
			set
			{
				if (value == ASC_ORDER ||
					value == DESC_ORDER)
				{
					_order = value;
				}
				else throw new ArgumentOutOfRangeException($"order must be \'{ASC_ORDER}\' or \'{DESC_ORDER}\'.");
			}
		}

		private int _limit = 1;
		public int limit { get => _limit;
			set
			{
				if (value >= MIN_LIMIT &&
					value <= MAX_LIMIT)
				{
					_limit = value;
				}
				else throw new ArgumentOutOfRangeException($"limit must be between {MIN_LIMIT} and {MAX_LIMIT}");
			}
		}
		private string? _sort = "";
		public string? sort { get => _sort;
			set
			{
				if (value == FILING_DATE_SORT ||
					value == PERIOD_OF_REPORT_DATE_SORT)
				{
					_sort = value;
				}
				else throw new ArgumentOutOfRangeException($"sort must be \'{FILING_DATE_SORT}\' or \'{PERIOD_OF_REPORT_DATE_SORT}\'");
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
			sb.Append( baseUrl );
			sb.Append( endpoint );
			sb.Append( $"?apiKey={apiKey}" );

			#region AddParams

			if (!String.IsNullOrEmpty(this.ticker))											sb.Append($"&ticker={this.ticker.ToUpper()}");
			if (!String.IsNullOrEmpty(this.cik))											sb.Append($"&cik={this.cik}");
			if (!String.IsNullOrEmpty(this.company_name))									sb.Append($"&company_name={this.company_name}");
			if (filing_date != null && !filing_date.IsNullOrEmpty())						sb.Append($"&{filing_date.nameof}={filing_date.value}");
			if (period_of_report_date != null && !period_of_report_date.IsNullOrEmpty())	sb.Append($"&{period_of_report_date.nameof}={period_of_report_date.value}");
			if (!String.IsNullOrEmpty(this.timeframe))										sb.Append($"&timeframe={this.timeframe}");
			if (this.include_sources != null)												sb.Append($"&include_sources={this.include_sources}");
			if (!String.IsNullOrEmpty(this.order))											sb.Append($"&order={this.order}");
																							sb.Append($"&limit={this.limit}");
			if (!String.IsNullOrEmpty(sort))												sb.Append($"&sort={this.sort}");

			#endregion

			return sb.ToString();
		}

	}
}
