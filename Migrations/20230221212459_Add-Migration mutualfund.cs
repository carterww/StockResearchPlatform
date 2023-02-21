using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationmutualfund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Stocks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "classID",
                table: "Stocks",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "seriesID",
                table: "Stocks",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "classID",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "seriesID",
                table: "Stocks");
        }
    }
}
