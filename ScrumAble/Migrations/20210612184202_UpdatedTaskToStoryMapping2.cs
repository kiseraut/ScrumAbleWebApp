using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class UpdatedTaskToStoryMapping2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Stories_StoryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_StoryId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "ScrumAbleStoryId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ScrumAbleStoryId",
                table: "Tasks",
                column: "ScrumAbleStoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Stories_ScrumAbleStoryId",
                table: "Tasks",
                column: "ScrumAbleStoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Stories_ScrumAbleStoryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ScrumAbleStoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ScrumAbleStoryId",
                table: "Tasks");

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
    }
}
