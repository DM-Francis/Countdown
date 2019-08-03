using Microsoft.EntityFrameworkCore.Migrations;

namespace Countdown.Website.Migrations
{
    public partial class RenameSolutionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Solutions",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Solutions",
                newName: "Result");
        }
    }
}
