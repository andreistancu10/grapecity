using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Scim_Risk_Db_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Risk",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    RiskCauses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskConsequences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbabilityOfApparitionEstimation = table.Column<int>(type: "int", nullable: false),
                    ImpactOfObjectivesEstimation = table.Column<int>(type: "int", nullable: false),
                    RiskExposureEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadOfDepartmentDecision = table.Column<int>(type: "int", nullable: false),
                    HeadOfDepartmentAssignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdoptedStrategy = table.Column<int>(type: "int", nullable: false),
                    AdoptedStrategyAssignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrategyDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilizedDocumentation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risk_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlAction",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RiskId = table.Column<long>(type: "bigint", nullable: false),
                    ControlMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlAction_Risk_RiskId",
                        column: x => x.RiskId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ActionId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ActivityId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlAction_RiskId",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskControlAction",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Risk",
                schema: "DocumentManagement");
        }
    }
}
