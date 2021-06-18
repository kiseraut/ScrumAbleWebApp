using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class contextchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "TaskOwnerId",
                table: "Tasks",
                newName: "TaskOwnerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskOwnerId",
                table: "Tasks",
                newName: "IX_Tasks_TaskOwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId1",
                table: "Tasks",
                column: "TaskOwnerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId1",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "TaskOwnerId1",
                table: "Tasks",
                newName: "TaskOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskOwnerId1",
                table: "Tasks",
                newName: "IX_Tasks_TaskOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_TaskOwnerId",
                table: "Tasks",
                column: "TaskOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
