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
        public override DividendInfo Retrieve(DividendInfo item)
        {
            throw new NotImplementedException();
        }

        public DividendInfo Retrieve(Stock FK_Stock)
        {
            throw new NotImplementedException();
        }

        public DividendInfo Retrive(Guid FK_Stock)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

