using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace StockResearchPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MutualFundClass> MutualFunds{ get; set;}
    }
}
