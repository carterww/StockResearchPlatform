using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class Stock
    {
		public Stock(string ticker)
		{
			Ticker = ticker;
		}

		[Key]
        public string Ticker { get; set; }
        [Required]
        public ulong CIK { get; set; }
    }
}
