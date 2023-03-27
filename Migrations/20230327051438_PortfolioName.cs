using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Portfolios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Portfolios");
        }
    }
}
