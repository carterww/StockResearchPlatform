using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockResearchPlatform.Models
{
    public class StockDividendLedger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Stock FK_Stock { get; set; }
        public DividendLedger FK_DividendLedger { get; set; }
        public double Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
