using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class changestock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MutualFunds_Stocks_Ticker",
                table: "MutualFunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MutualFunds",
                table: "MutualFunds");

            migrationBuilder.DropColumn(
                name: "Ticker",
                table: "MutualFunds");

            migrationBuilder.AlterColumn<string>(
                name: "Ticker",
                table: "Stocks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Stocks",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MutualFunds",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MutualFunds",
                table: "MutualFunds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MutualFunds_Stocks_Id",
                table: "MutualFunds",
                column: "Id",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MutualFunds_Stocks_Id",
                table: "MutualFunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MutualFunds",
                table: "MutualFunds");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MutualFunds");

            migrationBuilder.AlterColumn<string>(
                name: "Ticker",
                table: "Stocks",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Ticker",
                table: "MutualFunds",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "Ticker");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MutualFunds",
                table: "MutualFunds",
                column: "Ticker");

            migrationBuilder.AddForeignKey(
                name: "FK_MutualFunds_Stocks_Ticker",
                table: "MutualFunds",
                column: "Ticker",
                principalTable: "Stocks",
                principalColumn: "Ticker",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
