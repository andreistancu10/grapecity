using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Scim_DB_Activity_and_Action : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityFunctionary_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionFunctionaries",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionFunctionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionFunctionaries_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ActivityId",
                schema: "DocumentManagement",
                table: "Action",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFunctionaries_ActionId",
                schema: "DocumentManagement",
                table: "ActionFunctionaries",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "Activity",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "Activity",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityFunctionary_ActivityId",
                schema: "DocumentManagement",
                table: "ActivityFunctionary",
                column: "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionFunctionaries",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ActivityFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Activity",
                schema: "DocumentManagement");
        }
    }
}
