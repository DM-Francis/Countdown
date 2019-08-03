using Microsoft.EntityFrameworkCore.Migrations;

namespace Countdown.Website.Migrations
{
    public partial class AddClosestDiffColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClosestDiff",
                table: "Problems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosestDiff",
                table: "Problems");
        }
    }
}
