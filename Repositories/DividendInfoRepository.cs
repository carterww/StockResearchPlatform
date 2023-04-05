using System;
using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Data;
using StockResearchPlatform.Models;

namespace StockResearchPlatform.Repositories
{

	public class DividendInfoRepository : BaseRepository<DividendInfo>
	{
		public DividendInfoRepository(ApplicationDbContext context) : base(context)
		{
		}

        #region CustomRetrieves
        public override DividendInfo? Retrieve(DividendInfo item)
        {
            throw new NotImplementedException();
        }

        public DividendInfo? Retrieve(Stock FK_Stock)
        {
            throw new NotImplementedException();
        }

        public DividendInfo? Retrieve(Guid FK_Stock)
        {
            return _context.DividendInfo
                .Where(d => d.FK_Stock == FK_Stock)
                .FirstOrDefault();
        }
        #endregion

        #region AddOrUpdate
        public void AddOrUpdate(DividendInfo item)
        {
            if (this.Retrieve(item.FK_Stock) != null)
            {
                this.Update(item);
                Console.WriteLine($"Updating");
            }
            else
            {
                this.Create(item);
				Console.WriteLine($"Creating");
			}
        }
		#endregion

		#region Iamlazy
        public User? GetUser(string uid)
        {
            return _context.Users
                .Where(u => u.Id == uid)
                .Include("DividendLedgers")
                .FirstOrDefault();
        }
		#endregion
	}
}

