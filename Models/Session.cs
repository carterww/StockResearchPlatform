using System.ComponentModel.DataAnnotations;

namespace StockResearchPlatform.Models
{
    public class Session
    {
        [Key]
        public string Id { get; set; }
        public User FK_User { get; set; }
        [DataType(DataType.Date)]
        public DateTime Expiration { get; set; }
        [DataType(DataType.Date)]
        public DateTime Creation { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastAccessed { get; set; }
    }
}
