using System;
using StockResearchPlatform.Services.Polygon;

namespace StockResearchPlatform.Services.DividendTracker
{
	public class DividendTracker : IDividendTracker
	{
        public readonly PolygonDividendService _dividendService;

		public DividendTracker(PolygonDividendService dividendService)
		{
            _dividendService = dividendService;
		}

        public void AddDividendToLegder()
        {
            throw new NotImplementedException();
        }

        public void UpdateDividendInfoRecords()
        {
            throw new NotImplementedException();
        }
    }
}

