using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo2_CapitalMarketStory.Migrations
{
    /// <inheritdoc />
    public partial class AddedStartEndYearImports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndYear",
                table: "Import",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "Import",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "Import");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "Import");
        }
    }
}
