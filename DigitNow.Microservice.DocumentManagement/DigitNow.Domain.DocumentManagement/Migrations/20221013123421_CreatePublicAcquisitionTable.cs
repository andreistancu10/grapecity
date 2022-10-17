using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class CreatePublicAcquisitionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicAcquisitionProject",
                schema: "DocumentManagement");
        }
    }
}
