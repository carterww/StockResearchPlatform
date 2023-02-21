using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Models;

namespace StockResearchPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
