namespace StockResearchPlatform.Models.PolygonModels
{
	public class PreviousCloseResult
	{
		public string T { get; set; }
		public double c { get; set; }
		public double h { get; set; }
		public double l { get; set; }
		public double o { get; set; }
		public long t { get; set; }
		public int v { get; set; }
		public double vw { get; set; }
	}

	public class PreviousCloseJto
	{
		public bool adjusted { get; set; }
		public int queryCount { get; set; }
		public string request_id { get; set; }
		public List<PreviousCloseResult> results { get; set; }
		public int resultsCount { get; set; }
		public string status { get; set; }
		public string ticker { get; set; }
	}
}
