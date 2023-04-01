using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockResearchPlatform.Models
{
	public class DividendInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public Stock Stock { get; set; }

		[ForeignKey("Stock")]
		public Guid FK_Stock { get; set; }
		public DateTime UpdatedOn { get; set; }
		public DateTime DeclarationDate { get; set; }
		public DateTime ExDividendDate { get; set; }
		public DateTime PayDate { get; set; }
		public DateTime RecordDate { get; set; }
		public double Cashamount { get; set; }
	}
}
