using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class AddReleaseToDefect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReleaseId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ReleaseId",
                table: "Defects",
                column: "ReleaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Releases_ReleaseId",
                table: "Defects",
                column: "ReleaseId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Releases_ReleaseId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_ReleaseId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "ReleaseId",
                table: "Defects");
        }
    }
}
