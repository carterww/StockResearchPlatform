using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class StockSearchModel
    {
        [Required]
        public string? Ticker { get; set; }
    }
}
