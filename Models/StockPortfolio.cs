using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class StockPortfolio
    {
        [Required]
        public Stock Stock { get; set; }
        [Required]
        public Portfolio Portfolio { get; set; }
        [ForeignKey("Stock")]
        public Guid FK_Stock { get; set; }
        [ForeignKey("Portfolio")]
        public int FK_Portfolio { get; set; }

        public int NumberOfShares { get; set; }
        public double CostBasis { get; set; }
    }
}
