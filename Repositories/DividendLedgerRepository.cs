using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;

namespace StockResearchPlatform.Repositories
{
    public class DividendLedgerRepository : BaseRepository<DividendLedger>
    {
        public DividendLedgerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override DividendLedger? Retrieve(DividendLedger item)
        {
            return _context.DividendLedgers
				.Include(d => d.StockDividendLedgers)
				.Where(d => d.Id == item.Id)
                .FirstOrDefault();
        }

        public ICollection<StockDividendLedger>? RetrieveEntries(DividendLedger item)
        {
            return _context.StockDividendLedgers
                .Include("FK_Stock")
                .Where(s => s.FK_DividendLedger.Id == item.Id)
                .ToList();
        }

        public void CreateEntry(StockDividendLedger entry, StockPortfolio s)
        {
            _context.Attach(entry.FK_DividendLedger.FK_User);
            _context.Attach(s);
            _context.Attach(s.Portfolio);
            _context.StockDividendLedgers.Add(entry);
            this.Save();
        }
    }
}
