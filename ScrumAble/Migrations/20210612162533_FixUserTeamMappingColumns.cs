using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class FixUserTeamMappingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId1",
                table: "UserTeamMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping");

            migrationBuilder.DropIndex(
                name: "IX_UserTeamMapping_UserId1",
                table: "UserTeamMapping");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserTeamMapping");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserTeamMapping",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "UserTeamMapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamMapping_UserId",
                table: "UserTeamMapping",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId",
                table: "UserTeamMapping",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId",
                table: "UserTeamMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping");

            migrationBuilder.DropIndex(
                name: "IX_UserTeamMapping_UserId",
                table: "UserTeamMapping");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserTeamMapping",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "UserTeamMapping",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserTeamMapping",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamMapping_UserId1",
                table: "UserTeamMapping",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId1",
                table: "UserTeamMapping",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
