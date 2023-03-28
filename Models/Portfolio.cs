using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StockPortfolio> StockPortfolios { get; set; }
        public User FK_User { get; set; }
		[ForeignKey("FK_User")]
		public string FK_UserId { get; set; }
    }
}
