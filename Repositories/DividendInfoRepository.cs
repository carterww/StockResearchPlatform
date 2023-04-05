using System;
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
            throw new NotImplementedException();
        }
        #endregion

        #region AddOrUpdate
        public void AddOrUpdate(DividendInfo item)
        {
            if (this.Retrieve(item.FK_Stock) != null)
            {
                this.Update(item);
            }
            else
            {
                this.Create(item);
            }
        }
        #endregion
    }
}

