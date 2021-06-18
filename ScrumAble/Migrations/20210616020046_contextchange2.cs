using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class contextchange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Releases_currentWorkingReleaseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sprints_currentWorkingSprintId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_currentWorkingTeamId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "currentWorkingTeamId",
                table: "AspNetUsers",
                newName: "CurrentWorkingTeamId");

            migrationBuilder.RenameColumn(
                name: "currentWorkingSprintId",
                table: "AspNetUsers",
                newName: "CurrentWorkingSprintId");

            migrationBuilder.RenameColumn(
                name: "currentWorkingReleaseId",
                table: "AspNetUsers",
                newName: "CurrentWorkingReleaseId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_currentWorkingTeamId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CurrentWorkingTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_currentWorkingSprintId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CurrentWorkingSprintId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_currentWorkingReleaseId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CurrentWorkingReleaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Releases_CurrentWorkingReleaseId",
                table: "AspNetUsers",
                column: "CurrentWorkingReleaseId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sprints_CurrentWorkingSprintId",
                table: "AspNetUsers",
                column: "CurrentWorkingSprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_CurrentWorkingTeamId",
                table: "AspNetUsers",
                column: "CurrentWorkingTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Releases_CurrentWorkingReleaseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sprints_CurrentWorkingSprintId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_CurrentWorkingTeamId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CurrentWorkingTeamId",
                table: "AspNetUsers",
                newName: "currentWorkingTeamId");

            migrationBuilder.RenameColumn(
                name: "CurrentWorkingSprintId",
                table: "AspNetUsers",
                newName: "currentWorkingSprintId");

            migrationBuilder.RenameColumn(
                name: "CurrentWorkingReleaseId",
                table: "AspNetUsers",
                newName: "currentWorkingReleaseId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CurrentWorkingTeamId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_currentWorkingTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CurrentWorkingSprintId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_currentWorkingSprintId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CurrentWorkingReleaseId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_currentWorkingReleaseId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_currentWorkingTeamId",
                table: "AspNetUsers",
                column: "currentWorkingTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
