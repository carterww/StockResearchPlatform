using Microsoft.EntityFrameworkCore;
using StockResearchPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace StockResearchPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPortfolio>()
                .HasKey(nameof(StockPortfolio.FK_Stock), nameof(StockPortfolio.FK_Portfolio));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DividendLedger> DividendLedgers { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<StockDividendLedger> StockDividendLedgers { get; set; }
        public DbSet<StockPortfolio> StockPortfolios { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<MutualFundClass> MutualFunds{ get; set; }
        public DbSet<DividendInfo> DividendInfo { get; set; }
    }
}
