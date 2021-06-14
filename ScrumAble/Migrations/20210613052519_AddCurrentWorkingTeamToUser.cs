using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class AddCurrentWorkingTeamToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currentWorkingTeamId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_currentWorkingTeamId",
                table: "AspNetUsers",
                column: "currentWorkingTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_currentWorkingTeamId",
                table: "AspNetUsers",
                column: "currentWorkingTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_currentWorkingTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_currentWorkingTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "currentWorkingTeamId",
                table: "AspNetUsers");
        }
    }
}
