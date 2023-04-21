using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class Checkpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StockPortfolios",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DividendInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FK_Stock = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeclarationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExDividendDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cashamount = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DividendInfo_Stocks_FK_Stock",
                        column: x => x.FK_Stock,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DividendInfo_FK_Stock",
                table: "DividendInfo",
                column: "FK_Stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendInfo");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StockPortfolios");
        }
    }
}
