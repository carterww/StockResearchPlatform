namespace StockResearchPlatform.Models.PolygonModels
{
	/// <summary>
	/// These are Polygon's variable names
	/// c = Close Price
	/// h = Highest Price
	/// l = Lowest Price
	/// n = Number of transactions in the aggregate window
	/// o = Open Pirce
	/// otc = Is aggregate for OTC ticker
	/// t = Unix timestamp for start of aggregate window
	/// v = trading volume
	/// vw = volume weighted average price
	/// </summary>
	/// <see cref="https://polygon.io/docs/stocks/get_v2_aggs_ticker__stocksticker__range__multiplier___timespan___from___to"/>
	public class AggregateResult
	{
		public double c { get; set; }
		public double h { get; set; }
		public double l { get; set; }
		public int n { get; set; }
		public double o { get; set; }
		public object t { get; set; }
		public int v { get; set; }
		public double vw { get; set; }
	}

	public class AggregatesBarsV2Jto
	{
		public bool adjusted { get; set; }
		public string next_url { get; set; }
		public int queryCount { get; set; }
		public string request_id { get; set; }
		public List<AggregateResult> results { get; set; }
		public int resultsCount { get; set; }
		public string status { get; set; }
		public string ticker { get; set; }
	}
}
