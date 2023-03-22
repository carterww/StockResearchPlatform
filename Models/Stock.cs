using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockResearchPlatform.Models
{
    public class Stock
    {
		public Stock(string ticker)
		{
			Ticker = ticker;
            Id = Guid.NewGuid();
		}

		[Key]
        public Guid Id { get; set; }
		[Column(TypeName = "varchar(10)")]
		public string Ticker { get; set; }
        [Required]
        public ulong CIK { get; set; }
        public virtual MutualFundClass MutualFund { get; set; }
    }
}
