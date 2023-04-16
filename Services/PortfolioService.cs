using System;
using StockResearchPlatform.Models;
using StockResearchPlatform.Data;
using Microsoft.EntityFrameworkCore;

namespace StockResearchPlatform.Services
{
	public class PortfolioService
	{
		ApplicationDbContext _context;
		StockService _stockService;
		public PortfolioService(ApplicationDbContext context, StockService stockService)
		{
			_context = context;
			_stockService = stockService;
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
		}

		public Portfolio? UpdatePortfolio(Portfolio portfolio)
		{
			_context.Portfolios.Update(portfolio);
			return this.GetPortfolio(portfolio.Id);
		}

		public bool RemovePortfolio(Portfolio portfolio)
		{
			try
			{
				_context.Portfolios.Remove(portfolio);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

        #endregion

		public async Task<Dictionary<Stock, StockPortfolio>> GetStocksFromPortfolio(Portfolio p)
		{
			Dictionary<Stock, StockPortfolio> result = new Dictionary<Stock, StockPortfolio>(10);
			foreach(var stockPort in p.StockPortfolios)
			{
				Stock tmp = await _stockService.GetStock(null, stockPort.FK_Stock);
				result.Add(tmp, stockPort);
			}
			return result;
		}

        #region PORTFOLIOSTOCK_CRUD

		public StockPortfolio? GetStockPortfolio(StockPortfolio stockPortfolio)
		{
			return _context.StockPortfolios
                .Where(s => s.FK_Stock == stockPortfolio.FK_Stock
                    && s.FK_Portfolio == stockPortfolio.FK_Portfolio)
                .First();
        }

		public List<StockPortfolio> GetStockPortfolios()
		{
			return _context.StockPortfolios
				.Include("Stock")
				.Include("Portfolio")
				.ToList();
		}

		public async Task<Dictionary<Guid, StockPortfolio>> GetStockPortfolios(string userId)
		{
			var usersPortfolios = _context.Portfolios
				.Where(p => p.FK_UserId == userId)
				.Include("StockPortfolios")
				.ToList();

			Dictionary<Guid, StockPortfolio> stocks = new Dictionary<Guid, StockPortfolio>();
			foreach (var portfolio in usersPortfolios)
			{
				if (portfolio != null)
				{
					var dic = await this.GetStocksFromPortfolio(portfolio);
					foreach(var item in dic)
					{
						stocks.TryAdd(item.Key.Id, item.Value);
					}
				}
			}

			return stocks;

		}

		public async Task<Portfolio?> AddStockToPortfolio(StockPortfolio stockPortfolio)
		{
			await _context.StockPortfolios.AddAsync(stockPortfolio);
			return this.GetPortfolio(stockPortfolio.FK_Portfolio);
		}

		public Portfolio? UpdateStockPortfolio(StockPortfolio stockPortfolio)
		{
			var entryToUpdate = this.GetStockPortfolio(stockPortfolio);
			if (entryToUpdate != null)
			{
				entryToUpdate.CostBasis = stockPortfolio.CostBasis;
				entryToUpdate.NumberOfShares = stockPortfolio.NumberOfShares;
			}
            return this.GetPortfolio(stockPortfolio.FK_Portfolio);
        }

		public void RemoveStockPortfolio(StockPortfolio stockPortfolio)
		{
			var tmp = this.GetStockPortfolio(stockPortfolio);
			if (tmp != null)
			{
				_context.StockPortfolios.Remove(stockPortfolio);
			}
		}

        #endregion

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
    }
}

