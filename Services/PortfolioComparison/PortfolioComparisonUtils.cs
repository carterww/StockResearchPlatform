namespace StockResearchPlatform.Services.PortfolioComparison
{
	internal static class PortfolioComparisonUtils
	{
		public static double CalculateReturn(double oldPrice, double newPrice, double numOfShares)
		{
			return (newPrice - oldPrice) * numOfShares;
		}

		public static double CalculateBeta(double[] baseValues, double[] toCompareValues, double baseAvgDailyChange, double toComapreAvgDailyChange)
		{
			var xys = Enumerable.Zip(baseValues, toCompareValues, (x, y) => new { x = x, y = y });

			double ysum = 0, xsum = 0;
			int i = 0;
			foreach (var xy in xys)
			{
				if (i == 0) continue;
				ysum += (xy.x - baseAvgDailyChange) * (xy.y - toComapreAvgDailyChange);
				xsum += (baseValues[i] - baseAvgDailyChange) * (baseValues[i] - baseAvgDailyChange);
				i++;
			}

			return ysum / xsum;
		}

		public static double CalculateAplha(double portfolioReturn, double riskFreeReturn, double beta, double marketReturn)
		{
			return portfolioReturn - riskFreeReturn - (beta * (marketReturn - riskFreeReturn));
		}

	}
}
