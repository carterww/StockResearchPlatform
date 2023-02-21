using System.ComponentModel.DataAnnotations;

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
        public string Ticker { get; set; }
        [Required]
        public ulong CIK { get; set; }
        public virtual MutualFundClass MutualFund { get; set; }
    }
}
