using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class AddCurrentWorkingReleaseAndSprintToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currentWorkingReleaseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "currentWorkingSprintId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_currentWorkingReleaseId",
                table: "AspNetUsers",
                column: "currentWorkingReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_currentWorkingSprintId",
                table: "AspNetUsers",
                column: "currentWorkingSprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Releases_currentWorkingReleaseId",
                table: "AspNetUsers",
                column: "currentWorkingReleaseId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sprints_currentWorkingSprintId",
                table: "AspNetUsers",
                column: "currentWorkingSprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Releases_currentWorkingReleaseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sprints_currentWorkingSprintId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_currentWorkingReleaseId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_currentWorkingSprintId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "currentWorkingReleaseId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "currentWorkingSprintId",
                table: "AspNetUsers");
        }
    }
}
