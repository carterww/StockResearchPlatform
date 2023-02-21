using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockResearchPlatform.Models
{
	public class MutualFundClass
	{
		public MutualFundClass(Guid Id)
		{
			this.Id = Id;
		}
		public string SeriesID { get; set; }
		public string ClassID { get; set; }
		[Required]
		public virtual Stock Stock { get; set; }
		[ForeignKey("Stock"), Key]
		public Guid Id { get; set; }
	}
}

