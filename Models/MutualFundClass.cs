using System;
using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
	public class MutualFundClass : Stock
	{
		public MutualFundClass(string ticker) : base(ticker)
		{
		}
		public string seriesID { get; set; }
		public string classID { get; set; }
	}
}

