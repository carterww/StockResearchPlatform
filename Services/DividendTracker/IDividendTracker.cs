using System;
namespace StockResearchPlatform.Services.DividendTracker
{
	public interface IDividendTracker
	{
		/// <summary>
		/// Goes through all stocks tracked by portfolios and adds/updates latest dividend
		/// declaration to table.
		/// </summary>
		public Task<bool> UpdateDividendInfoRecords();

		/// <summary>
		/// Adds dividends whose pay date <= today's date to all portfolio's
		/// dividend ledgers that have that stock in their portfolio
		/// </summary>
		public void AddDividendToLegder();

	}
}

