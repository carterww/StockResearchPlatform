namespace StockResearchPlatform.Models.PortfolioDtos
{
	public class PortfolioComparisonDto
	{
		/// <summary>
		/// Dictionary that has portfolio's ID as Key and the return as Value
		/// </summary>
		public Dictionary<int, double> Returns { get; set; } = new Dictionary<int, double>();
		public int NumberOfMonths { get; set; } = 60;
		public Dictionary<int, PortfolioComparisonHelperDto> Values { get; set; } = new Dictionary<int, PortfolioComparisonHelperDto>();
	}

	public class PortfolioComparisonHelperDto
	{
		public int PortfolioId { get; set; }
		public double[] PortfolioPriceHistory { get; set; }
		public double[] PortfolioPriceHistoryChange { get; set; }
		public double PortfolioAverageChange { get; set; }
		public double Beta { get; set; }
		public double Alpha { get; set; }
		public double TotalReturn { get; set; }
	}
}
