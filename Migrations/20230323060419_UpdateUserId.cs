using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DividendLedgers_Users_FK_UserEmail",
                table: "DividendLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Users_FK_UserEmail",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_FK_UserEmail",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_FK_UserEmail",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_FK_UserEmail",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_DividendLedgers_FK_UserEmail",
                table: "DividendLedgers");

            migrationBuilder.DropColumn(
                name: "FK_UserEmail",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "FK_UserEmail",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "FK_UserEmail",
                table: "DividendLedgers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "FK_UserId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_UserId",
                table: "Portfolios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FK_UserId",
                table: "DividendLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FK_UserId",
                table: "Sessions",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_FK_UserId",
                table: "Portfolios",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendLedgers_FK_UserId",
                table: "DividendLedgers",
                column: "FK_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DividendLedgers_Users_FK_UserId",
                table: "DividendLedgers",
                column: "FK_UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Users_FK_UserId",
                table: "Portfolios",
                column: "FK_UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_FK_UserId",
                table: "Sessions",
                column: "FK_UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DividendLedgers_Users_FK_UserId",
                table: "DividendLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Users_FK_UserId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_FK_UserId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_FK_UserId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_FK_UserId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_DividendLedgers_FK_UserId",
                table: "DividendLedgers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FK_UserId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "FK_UserId",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "FK_UserId",
                table: "DividendLedgers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FK_UserEmail",
                table: "Sessions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FK_UserEmail",
                table: "Portfolios",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FK_UserEmail",
                table: "DividendLedgers",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FK_UserEmail",
                table: "Sessions",
                column: "FK_UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_FK_UserEmail",
                table: "Portfolios",
                column: "FK_UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_DividendLedgers_FK_UserEmail",
                table: "DividendLedgers",
                column: "FK_UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_DividendLedgers_Users_FK_UserEmail",
                table: "DividendLedgers",
                column: "FK_UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Users_FK_UserEmail",
                table: "Portfolios",
                column: "FK_UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_FK_UserEmail",
                table: "Sessions",
                column: "FK_UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
