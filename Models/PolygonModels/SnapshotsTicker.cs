namespace StockResearchPlatform.Models.PolygonModels
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Day
	{
		public double o { get; set; }
		public double h { get; set; }
		public double l { get; set; }
		public double c { get; set; }
		public double v { get; set; }
		public double vw { get; set; }
	}

	public class Min
	{
		public double av { get; set; }
		public double t { get; set; }
		public double o { get; set; }
		public double h { get; set; }
		public double l { get; set; }
		public double c { get; set; }
		public double v { get; set; }
		public double vw { get; set; }
	}

	public class PrevDay
	{
		public double o { get; set; }
		public double h { get; set; }
		public double l { get; set; }
		public double c { get; set; }
		public double v { get; set; }
		public double vw { get; set; }
	}

	public class SnapshotsTickerV2Jto
	{
		public NestedTicker ticker { get; set; }
		public string status { get; set; }
		public string request_id { get; set; }
	}

	public class NestedTicker
	{
		public string ticker { get; set; }
		public double todaysChangePerc { get; set; }
		public double todaysChange { get; set; }
		public double updated { get; set; }
		public Day day { get; set; }
		public Min min { get; set; }
		public PrevDay prevDay { get; set; }
	}

}
