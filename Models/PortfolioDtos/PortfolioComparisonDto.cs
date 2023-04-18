namespace StockResearchPlatform.Models.PortfolioDtos
{
	public class PortfolioComparisonDto
	{
		/// <summary>
		/// Dictionary that has portfolio's ID as Key and the return as Value
		/// </summary>
		public Dictionary<int, double> Returns { get; set; } = new Dictionary<int, double>();
		public int NumberOfMonths { get; set; } = 60;
	}
}
