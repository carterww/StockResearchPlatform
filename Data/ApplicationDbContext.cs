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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPortfolio>()
                .HasKey(nameof(StockPortfolio.FK_Stock), nameof(StockPortfolio.FK_Portfolio));
        }

        public DbSet<DividendLedger> DividendLedgers { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<StockDividendLedger> StockDividendLedgers { get; set; }
        public DbSet<StockPortfolio> StockPortfolios { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MutualFundClass> MutualFunds{ get; set;}
    }
}
