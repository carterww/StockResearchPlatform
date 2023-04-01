// TickerDetailsJto is a class that gets mapped from json returned by PolyGonTickerService.TickerDetails()
// Class is used for serializing to json and deserializing from json

namespace StockResearchPlatform.Models.PolygonModels
{
	// TickerDetailsV3Jto myDeserializedClass = JsonConvert.DeserializeObject<TickerDetailsV3Jto>(myJsonResponse);
	public class Address
	{
		public string address1 { get; set; }
		public string city { get; set; }
		public string postal_code { get; set; }
		public string state { get; set; }
	}

	public class Branding
	{
		public string icon_url { get; set; }
		public string logo_url { get; set; }
	}

	public class Results
	{
		public bool active { get; set; }
		public Address address { get; set; }
		public Branding branding { get; set; }
		public string cik { get; set; }
		public string composite_figi { get; set; }
		public string currency_name { get; set; }
		public string description { get; set; }
		public string homepage_url { get; set; }
		public string list_date { get; set; }
		public string locale { get; set; }
		public string market { get; set; }
		public long market_cap { get; set; }
		public string name { get; set; }
		public string phone_number { get; set; }
		public string primary_exchange { get; set; }
		public int round_lot { get; set; }
		public string share_class_figi { get; set; }
		public long share_class_shares_outstanding { get; set; }
		public string sic_code { get; set; }
		public string sic_description { get; set; }
		public string ticker { get; set; }
		public string ticker_root { get; set; }
		public int total_employees { get; set; }
		public string type { get; set; }
		public long weighted_shares_outstanding { get; set; }
	}

	public class TickerDetailsV3Jto
	{
		public string request_id { get; set; }
		public Results results { get; set; }
		public string status { get; set; }
	}
}
