using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class fgx4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralObjectives_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjectives");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjectives_GeneralObjectives_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjectives_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificObjectives",
                schema: "DocumentMangement",
                table: "SpecificObjectives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralObjectives",
                schema: "DocumentMangement",
                table: "GeneralObjectives");

            migrationBuilder.DropColumn(
                name: "FunctionaryId",
                schema: "DocumentMangement",
                table: "SpecificObjectives");

            migrationBuilder.RenameTable(
                name: "SpecificObjectives",
                schema: "DocumentMangement",
                newName: "SpecificObjective",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "GeneralObjectives",
                schema: "DocumentMangement",
                newName: "GeneralObjective",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificObjectives_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                newName: "IX_SpecificObjective_ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificObjectives_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                newName: "IX_SpecificObjective_GeneralObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralObjectives_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjective",
                newName: "IX_GeneralObjective_ObjectiveId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificationMotive",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificObjective",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralObjective",
                schema: "DocumentMangement",
                table: "GeneralObjective",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SpecificObjectiveFunctionary",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificObjectiveFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificObjectiveFunctionary_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "SpecificObjective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjectiveFunctionary_SpecificObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectiveFunctionary",
                column: "SpecificObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjective",
                column: "ObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificObjective_GeneralObjective_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "GeneralObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "GeneralObjective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "ObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjective");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjective_GeneralObjective_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective");

            migrationBuilder.DropTable(
                name: "SpecificObjectiveFunctionary",
                schema: "DocumentMangement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificObjective",
                schema: "DocumentMangement",
                table: "SpecificObjective");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralObjective",
                schema: "DocumentMangement",
                table: "GeneralObjective");

            migrationBuilder.DropColumn(
                name: "ModificationMotive",
                schema: "DocumentMangement",
                table: "Objective");

            migrationBuilder.RenameTable(
                name: "SpecificObjective",
                schema: "DocumentMangement",
                newName: "SpecificObjectives",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "GeneralObjective",
                schema: "DocumentMangement",
                newName: "GeneralObjectives",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificObjective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                newName: "IX_SpecificObjectives_ObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificObjective_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                newName: "IX_SpecificObjectives_GeneralObjectiveId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralObjective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjectives",
                newName: "IX_GeneralObjectives_ObjectiveId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "DocumentMangement",
                table: "Objective",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "FunctionaryId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificObjectives",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralObjectives",
                schema: "DocumentMangement",
                table: "GeneralObjectives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralObjectives_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjectives",
                column: "ObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificObjectives_GeneralObjectives_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                column: "GeneralObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "GeneralObjectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificObjectives_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                column: "ObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
