using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AddPerformanceIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceIndicator",
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
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantificationFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultIndicator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolutionStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceIndicator_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceIndicator_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceIndicator_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicAcquisitionProject",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ProjectYear = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpvCode = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedValue = table.Column<float>(type: "real", nullable: false),
                    FinancingSource = table.Column<long>(type: "bigint", nullable: false),
                    EstablishedProcedure = table.Column<long>(type: "bigint", nullable: false),
                    EstimatedMonthForInitiatingProcedure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedMonthForProcedureAssignment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcedureAssignmentMethod = table.Column<long>(type: "bigint", nullable: false),
                    ProcedureAssignmentResponsible = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicAcquisitionProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceIndicatorFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    PerformanceIndicatorId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceIndicatorFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceIndicatorFunctionary_PerformanceIndicator_PerformanceIndicatorId",
                        column: x => x.PerformanceIndicatorId,
                        principalSchema: "DocumentManagement",
                        principalTable: "PerformanceIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceIndicator_ActivityId",
                schema: "DocumentManagement",
                table: "PerformanceIndicator",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceIndicator_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "PerformanceIndicator",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceIndicator_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "PerformanceIndicator",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceIndicatorFunctionary_PerformanceIndicatorId",
                schema: "DocumentManagement",
                table: "PerformanceIndicatorFunctionary",
                column: "PerformanceIndicatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerformanceIndicatorFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "PublicAcquisitionProject",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "PerformanceIndicator",
                schema: "DocumentManagement");
        }
    }
}
