namespace StockResearchPlatform.Models.PolygonModels
{
	// DividendsV3Jto myDeserializedClass = JsonConvert.DeserializeObject<DividendsV3Jto>(myJsonResponse);
	public class DividendResult
	{
		public double cash_amount { get; set; }
		public string declaration_date { get; set; }
		public string dividend_type { get; set; }
		public string ex_dividend_date { get; set; }
		public int frequency { get; set; }
		public string pay_date { get; set; }
		public string record_date { get; set; }
		public string ticker { get; set; }
	}

	public class DividendsV3Jto
	{
		public string next_url { get; set; }
		public List<DividendResult> results { get; set; }
		public string status { get; set; }
	}
}
