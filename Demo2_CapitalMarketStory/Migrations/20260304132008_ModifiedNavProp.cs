using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo2_CapitalMarketStory.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedNavProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Import_Company_CompanyId",
                table: "Import");

            migrationBuilder.DropForeignKey(
                name: "FK_YearlyFinancialReport_Import_ImportId",
                table: "YearlyFinancialReport");

            migrationBuilder.AlterColumn<int>(
                name: "ImportId",
                table: "YearlyFinancialReport",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Import",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Import_Company_CompanyId",
                table: "Import",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_YearlyFinancialReport_Import_ImportId",
                table: "YearlyFinancialReport",
                column: "ImportId",
                principalTable: "Import",
                principalColumn: "ImportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Import_Company_CompanyId",
                table: "Import");

            migrationBuilder.DropForeignKey(
                name: "FK_YearlyFinancialReport_Import_ImportId",
                table: "YearlyFinancialReport");

            migrationBuilder.AlterColumn<int>(
                name: "ImportId",
                table: "YearlyFinancialReport",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Import",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Import_Company_CompanyId",
                table: "Import",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YearlyFinancialReport_Import_ImportId",
                table: "YearlyFinancialReport",
                column: "ImportId",
                principalTable: "Import",
                principalColumn: "ImportId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
