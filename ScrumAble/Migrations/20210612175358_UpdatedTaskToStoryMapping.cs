using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class UpdatedTaskToStoryMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoryID",
                table: "Tasks",
                newName: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StoryId",
                table: "Tasks",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Stories_StoryId",
                table: "Tasks",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Stories_StoryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_StoryId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "Tasks",
                newName: "StoryID");
        }
    }
}
