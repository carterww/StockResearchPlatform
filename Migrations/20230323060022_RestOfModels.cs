using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class RestOfModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DividendLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FK_UserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DividendLedgers_Users_FK_UserEmail",
                        column: x => x.FK_UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FK_UserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_FK_UserEmail",
                        column: x => x.FK_UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_UserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_FK_UserEmail",
                        column: x => x.FK_UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockDividendLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FK_StockId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FK_DividendLedgerId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDividendLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockDividendLedgers_DividendLedgers_FK_DividendLedgerId",
                        column: x => x.FK_DividendLedgerId,
                        principalTable: "DividendLedgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockDividendLedgers_Stocks_FK_StockId",
                        column: x => x.FK_StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockPortfolios",
                columns: table => new
                {
                    FK_Stock = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FK_Portfolio = table.Column<int>(type: "int", nullable: false),
                    NumberOfShares = table.Column<int>(type: "int", nullable: false),
                    CostBasis = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPortfolios", x => new { x.FK_Stock, x.FK_Portfolio });
                    table.ForeignKey(
                        name: "FK_StockPortfolios_Portfolios_FK_Portfolio",
                        column: x => x.FK_Portfolio,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockPortfolios_Stocks_FK_Stock",
                        column: x => x.FK_Stock,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DividendLedgers_FK_UserEmail",
                table: "DividendLedgers",
                column: "FK_UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_FK_UserEmail",
                table: "Portfolios",
                column: "FK_UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FK_UserEmail",
                table: "Sessions",
                column: "FK_UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_StockDividendLedgers_FK_DividendLedgerId",
                table: "StockDividendLedgers",
                column: "FK_DividendLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDividendLedgers_FK_StockId",
                table: "StockDividendLedgers",
                column: "FK_StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPortfolios_FK_Portfolio",
                table: "StockPortfolios",
                column: "FK_Portfolio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "StockDividendLedgers");

            migrationBuilder.DropTable(
                name: "StockPortfolios");

            migrationBuilder.DropTable(
                name: "DividendLedgers");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
