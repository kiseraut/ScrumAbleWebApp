using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumAble.Migrations
{
    public partial class AddDefect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefectOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DefectStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefectDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefectCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefectPoints = table.Column<int>(type: "int", nullable: false),
                    DefectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowStageId = table.Column<int>(type: "int", nullable: true),
                    SprintId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_AspNetUsers_DefectOwnerId",
                        column: x => x.DefectOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Defects_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Defects_WorkflowStages_WorkflowStageId",
                        column: x => x.WorkflowStageId,
                        principalTable: "WorkflowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Defects_DefectOwnerId",
                table: "Defects",
                column: "DefectOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_SprintId",
                table: "Defects",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_WorkflowStageId",
                table: "Defects",
                column: "WorkflowStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Defects");
        }
    }
}
