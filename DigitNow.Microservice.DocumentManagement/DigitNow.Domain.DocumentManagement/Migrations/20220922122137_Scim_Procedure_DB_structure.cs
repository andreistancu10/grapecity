using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Scim_Procedure_DB_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Procedure",
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
                    ProcedureCategory = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainOfApplicability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternationalReglementation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryLegislation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryLegislation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherReglementationn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefinitionsAndAbbreviations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsibility = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procedure_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Procedure_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Procedure_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ProcedureId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureFunctionary_Procedure_ProcedureId",
                        column: x => x.ProcedureId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_ActivityId",
                schema: "DocumentManagement",
                table: "Procedure",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "Procedure",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "Procedure",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureFunctionary_ProcedureId",
                schema: "DocumentManagement",
                table: "ProcedureFunctionary",
                column: "ProcedureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcedureFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Procedure",
                schema: "DocumentManagement");
        }
    }
}
