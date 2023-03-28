using System;
using StockResearchPlatform.Models;
using StockResearchPlatform.Data;
using Microsoft.EntityFrameworkCore;

namespace StockResearchPlatform.Services
{
	public class PortfolioService
	{
		ApplicationDbContext _context;
		public PortfolioService(ApplicationDbContext context)
		{
			_context = context;
		}

        #region PORTFOLIO_CRUD

        public Portfolio? GetPortfolio(int Id)
		{
			return _context.Portfolios
				.Where(p => p.Id == Id)
				.Include("StockPortfolios")
				.First();
		}

		public async Task AddPortfolio(Portfolio portfolio)
		{
			await _context.Portfolios.AddAsync(portfolio);
			this.SaveChanges();
		}

		public async Task<Portfolio?> UpdatePortfolio(Portfolio portfolio)
		{
			_context.Portfolios.Update(portfolio);
			this.SaveChanges();
			return this.GetPortfolio(portfolio.Id);
		}

		public bool DeletePortfolio(Portfolio portfolio)
		{
			try
			{
				_context.Portfolios.Remove(portfolio);
				this.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

        #endregion

        #region PORTFOLIOSTOCK_CRUD

		public async Task<Portfolio?> AddStockToPortfolio(StockPortfolio stockPortfolio)
		{
			await _context.StockPortfolios.AddAsync(stockPortfolio);
			this.SaveChanges();
			return this.GetPortfolio(stockPortfolio.FK_Portfolio);
		}

        #endregion

		private void SaveChanges()
		{
			_context.SaveChanges();
		}
    }
}

