using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo2_CapitalMarketStory.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Company");
        }
    }
}
