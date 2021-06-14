using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class UpdatedDBRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId",
                table: "UserTeamMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping");

            migrationBuilder.DropColumn(
                name: "ReleaseId",
                table: "Sprints");

            migrationBuilder.RenameColumn(
                name: "ReleaseName",
                table: "WorkflowStages",
                newName: "WorkflowStageName");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageID",
                table: "Tasks",
                newName: "WorkflowStageId");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageID",
                table: "Stories",
                newName: "WorkflowStageId");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "WorkflowStages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserTeamMapping",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
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

            migrationBuilder.AddColumn<int>(
                name: "ReleaseIdId",
                table: "Sprints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Releases",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStages_TeamId",
                table: "WorkflowStages",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SprintId",
                table: "Tasks",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkflowStageId",
                table: "Tasks",
                column: "WorkflowStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_SprintId",
                table: "Stories",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_WorkflowStageId",
                table: "Stories",
                column: "WorkflowStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ReleaseIdId",
                table: "Sprints",
                column: "ReleaseIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_TeamId",
                table: "Releases",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_Teams_TeamId",
                table: "Releases",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Releases_ReleaseIdId",
                table: "Sprints",
                column: "ReleaseIdId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Sprints_SprintId",
                table: "Stories",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_WorkflowStages_WorkflowStageId",
                table: "Stories",
                column: "WorkflowStageId",
                principalTable: "WorkflowStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_WorkflowStages_WorkflowStageId",
                table: "Tasks",
                column: "WorkflowStageId",
                principalTable: "WorkflowStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId",
                table: "UserTeamMapping",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStages_Teams_TeamId",
                table: "WorkflowStages",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Releases_Teams_TeamId",
                table: "Releases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Releases_ReleaseIdId",
                table: "Sprints");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Sprints_SprintId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_WorkflowStages_WorkflowStageId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_WorkflowStages_WorkflowStageId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_AspNetUsers_UserId",
                table: "UserTeamMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamMapping_Teams_TeamId",
                table: "UserTeamMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStages_Teams_TeamId",
                table: "WorkflowStages");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowStages_TeamId",
                table: "WorkflowStages");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SprintId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_WorkflowStageId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Stories_SprintId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Stories_WorkflowStageId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_ReleaseIdId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Releases_TeamId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "ReleaseIdId",
                table: "Sprints");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageName",
                table: "WorkflowStages",
                newName: "ReleaseName");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageId",
                table: "Tasks",
                newName: "WorkflowStageID");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageId",
                table: "Stories",
                newName: "WorkflowStageID");

            migrationBuilder.AlterColumn<string>(
                name: "TeamId",
                table: "WorkflowStages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserTeamMapping",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "UserTeamMapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ReleaseId",
                table: "Sprints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TeamId",
                table: "Releases",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
