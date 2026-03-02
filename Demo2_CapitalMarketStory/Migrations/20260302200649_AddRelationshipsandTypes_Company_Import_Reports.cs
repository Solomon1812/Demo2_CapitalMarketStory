using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo2_CapitalMarketStory.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipsandTypes_Company_Import_Reports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    ImportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.ImportId);
                    table.ForeignKey(
                        name: "FK_Import_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YearlyFinancialReport",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUI = table.Column<int>(type: "int", nullable: false),
                    YearReported = table.Column<int>(type: "int", nullable: false),
                    ActiveImobilizate = table.Column<double>(type: "float", nullable: false),
                    ActiveCirculante = table.Column<double>(type: "float", nullable: false),
                    Stocuri = table.Column<double>(type: "float", nullable: false),
                    Creante = table.Column<double>(type: "float", nullable: false),
                    Casa = table.Column<double>(type: "float", nullable: false),
                    CheltuieliAvans = table.Column<double>(type: "float", nullable: false),
                    Datorii = table.Column<double>(type: "float", nullable: false),
                    VenituriAvans = table.Column<double>(type: "float", nullable: false),
                    Provizioane = table.Column<double>(type: "float", nullable: false),
                    CapitaluriTotale = table.Column<double>(type: "float", nullable: false),
                    CapitaluriVarsate = table.Column<double>(type: "float", nullable: false),
                    Patrimoniu = table.Column<double>(type: "float", nullable: false),
                    CifraAfaceriNet = table.Column<double>(type: "float", nullable: false),
                    VenituriTotale = table.Column<double>(type: "float", nullable: false),
                    CheltuieliTotale = table.Column<double>(type: "float", nullable: false),
                    ProfitBrut = table.Column<double>(type: "float", nullable: false),
                    PierdereBrut = table.Column<double>(type: "float", nullable: false),
                    ProfitNet = table.Column<double>(type: "float", nullable: false),
                    PierdereNet = table.Column<double>(type: "float", nullable: false),
                    NumarSalariati = table.Column<int>(type: "int", nullable: false),
                    ROA = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    ROE = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    MarjaProfit = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    RataCrestereCifraAfaceriNet = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    RataCrestereProfitNet = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    ImportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyFinancialReport", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_YearlyFinancialReport_Import_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Import",
                        principalColumn: "ImportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Import_CompanyId",
                table: "Import",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyFinancialReport_ImportId",
                table: "YearlyFinancialReport",
                column: "ImportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YearlyFinancialReport");

            migrationBuilder.DropTable(
                name: "Import");
        }
    }
}
