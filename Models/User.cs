using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockResearchPlatform.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<DividendLedger> DividendLedgers { get; set; }
        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
