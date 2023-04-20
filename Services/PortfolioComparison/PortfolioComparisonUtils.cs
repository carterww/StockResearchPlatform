namespace StockResearchPlatform.Services.PortfolioComparison
{
	internal static class PortfolioComparisonUtils
	{
		public static double CalculateReturn(double oldPrice, double newPrice, double numOfShares)
		{
			return (newPrice - oldPrice) / oldPrice;
		}

		public static double CalculateBeta(double[] xArray, double[] yArray)
		{
			double n = xArray.Length;
			double sumxy = 0, sumx = 0, sumy = 0, sumx2 = 0;
			for (int i = 1; i < xArray.Length; i++)
			{
				sumxy += xArray[i] * yArray[i];
				sumx += xArray[i];
				sumy += yArray[i];
				sumx2 += xArray[i] * xArray[i];
			}
			return ((sumxy - sumx * sumy / n) / (sumx2 - sumx * sumx / n));
		}

		public static double CalculateJensensAplha(double portfolioReturn, double riskFreeReturn, double beta, double marketReturn)
		{
			return portfolioReturn - (riskFreeReturn + (beta * (marketReturn - riskFreeReturn)));
		}

		public static double CalculateAlpha(double portfolioReturn, double marketReturn)
		{
			return portfolioReturn - marketReturn;
		}

	}
}
