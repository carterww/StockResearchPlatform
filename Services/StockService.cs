using System;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;

namespace StockResearchPlatform.Services
{
	public class StockService
	{
		ApplicationDbContext _context;

		public StockService(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<Stock?> GetStock(string? ticker, System.Guid? Id)
		{
			if (Id == null && ticker != null)
			{
				return _context.Stocks.Where(s => s.Ticker == ticker).First();
			}
			else if (Id != null)
			{
                return _context.Stocks.Find(Id);
            }
			return null;
		}


    }
}

