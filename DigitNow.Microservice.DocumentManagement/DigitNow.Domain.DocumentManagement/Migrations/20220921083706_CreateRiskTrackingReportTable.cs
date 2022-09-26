using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class CreateRiskTrackingReportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskTrackingReport",
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
                    ControlMeasuresImplementationState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbabilityOfApparitionEstimation = table.Column<int>(type: "int", nullable: false),
                    ImpactOfObjectivesEstimation = table.Column<int>(type: "int", nullable: false),
                    RiskExposureEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTrackingReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskTrackingReport_Risk_RiskId",
                        column: x => x.RiskId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskActionProposal",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RiskTrackingReportId = table.Column<long>(type: "bigint", nullable: false),
                    ProposedAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskTrackingReportDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskActionProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskActionProposal_RiskTrackingReport_RiskTrackingReportId",
                        column: x => x.RiskTrackingReportId,
                        principalSchema: "DocumentManagement",
                        principalTable: "RiskTrackingReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskActionProposal_RiskTrackingReportId",
                schema: "DocumentManagement",
                table: "RiskActionProposal",
                column: "RiskTrackingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTrackingReport_RiskId",
                schema: "DocumentManagement",
                table: "RiskTrackingReport",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskActionProposal",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "RiskTrackingReport",
                schema: "DocumentManagement");
        }
    }
}
