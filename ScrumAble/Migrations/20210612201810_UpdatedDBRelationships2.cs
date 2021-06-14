using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class UpdatedDBRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Releases_ReleaseIdId",
                table: "Sprints");

            migrationBuilder.RenameColumn(
                name: "ReleaseIdId",
                table: "Sprints",
                newName: "ReleaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Sprints_ReleaseIdId",
                table: "Sprints",
                newName: "IX_Sprints_ReleaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Releases_ReleaseId",
                table: "Sprints",
                column: "ReleaseId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Releases_ReleaseId",
                table: "Sprints");

            migrationBuilder.RenameColumn(
                name: "ReleaseId",
                table: "Sprints",
                newName: "ReleaseIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Sprints_ReleaseId",
                table: "Sprints",
                newName: "IX_Sprints_ReleaseIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Releases_ReleaseIdId",
                table: "Sprints",
                column: "ReleaseIdId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
