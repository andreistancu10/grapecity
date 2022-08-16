using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class fg3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "ObjectiveId",
                principalSchema: "DocumentMangement",
                principalTable: "Objective",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificObjective_Objective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective");

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
    }
}
