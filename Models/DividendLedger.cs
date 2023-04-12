using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class DividendLedger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		public User FK_User { get; set; }
		[ForeignKey("FK_User")]
		public string FK_UserId { get; set; }
		public virtual ICollection<StockDividendLedger> StockDividendLedgers { get; set; }
    }
}
