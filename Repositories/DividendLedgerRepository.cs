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
                .Where(d => d.Id == item.Id)
                .Include("StockDividendLedgers")
                .FirstOrDefault();
        }

        public void CreateEntry(StockDividendLedger entry)
        {
            _context.StockDividendLedgers.Add(entry);
        }
    }
}
