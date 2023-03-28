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

        public async Task<Stock?> GetStock(System.Guid Id)
		{
			return _context.Stocks.Find(Id);
		}


    }
}

