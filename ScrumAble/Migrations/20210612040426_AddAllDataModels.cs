using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class AddAllDataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "Tasks",
                newName: "WorkflowStageID");

            migrationBuilder.RenameColumn(
                name: "ReleaseId",
                table: "Tasks",
                newName: "TaskPoints");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Tasks",
                newName: "TaskStartDate");

            migrationBuilder.AddColumn<int>(
                name: "StoryID",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskCloseDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TaskDescription",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskDueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TaskOwner",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleaseEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprintName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SprintStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SprintEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleaseId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoryOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoryStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoryDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoryCloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoryPoints = table.Column<int>(type: "int", nullable: false),
                    StoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowStageID = table.Column<int>(type: "int", nullable: true),
                    SprintId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowStagePosition = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTeamMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeamMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeamMapping_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeamMapping_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamMapping_TeamId",
                table: "UserTeamMapping",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamMapping_UserId1",
                table: "UserTeamMapping",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "UserTeamMapping");

            migrationBuilder.DropTable(
                name: "WorkflowStages");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropColumn(
                name: "StoryID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskCloseDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskDescription",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskDueDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskOwner",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "WorkflowStageID",
                table: "Tasks",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "TaskStartDate",
                table: "Tasks",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "TaskPoints",
                table: "Tasks",
                newName: "ReleaseId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
