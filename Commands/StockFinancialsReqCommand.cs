namespace StockResearchPlatform.Commands
{
	public class StockFinancialsReqCommand
	{
		#region TimeframeConstants
		public const string ANNUAL_TIMEFRAME = "annual";
		public const string QUARTERLY_TIMEFRAME = "quarterly";
		public const string TTM_TIMEFRAME = "ttm";
		#endregion

		#region OrderConstants
		public const string ASC_ORDER = "asc";
		public const string DESC_ORDER = "desc";
		#endregion

		#region LimitConstants
		public const int MIN_LIMIT = 1;
		public const int MAX_LIMIT = 100;
		public const int DEFAULT_LIMIT = 10;
		#endregion

		#region SortConstants
		public const string FILING_DATE_SORT = "filing_date";
		public const string PERIOD_OF_REPORT_DATE_SORT = "period_of_report_date";
		#endregion

		public string ticker { get; set; } = "";
		public string cik { get; set; } = "";
		public string company_name { get; set; } = "";
		public PolygonParamInequality filing_date { get; set; }
		public PolygonParamInequality period_of_report_date { get; set; }
		private string _timeframe = "";
		public string timeframe { get => _timeframe;
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

		public bool include_sources { get; set; } = false;

		private string _order = "";
		public string order { get => _order;
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
		private string _sort = "";
		public string sort { get => _sort;
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



	}
}
