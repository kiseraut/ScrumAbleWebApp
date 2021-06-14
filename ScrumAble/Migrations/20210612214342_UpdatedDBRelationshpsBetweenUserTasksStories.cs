using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class UpdatedDBRelationshpsBetweenUserTasksStories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskOwner",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StoryOwner",
                table: "Stories");

            migrationBuilder.AddColumn<string>(
                name: "TaskOwnerId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoryOwnerId",
                table: "Stories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskOwnerId",
                table: "Tasks",
                column: "TaskOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_StoryOwnerId",
                table: "Stories",
                column: "StoryOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AspNetUsers_StoryOwnerId",
                table: "Stories",
                column: "StoryOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId",
                table: "Tasks",
                column: "TaskOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_StoryOwnerId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskOwnerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Stories_StoryOwnerId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "TaskOwnerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StoryOwnerId",
                table: "Stories");

            migrationBuilder.AddColumn<string>(
                name: "TaskOwner",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoryOwner",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
