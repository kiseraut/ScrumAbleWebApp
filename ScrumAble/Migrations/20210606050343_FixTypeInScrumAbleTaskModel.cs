using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class FixTypeInScrumAbleTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RleaseId",
                table: "Tasks",
                newName: "ReleaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReleaseId",
                table: "Tasks",
                newName: "RleaseId");
        }
    }
}
