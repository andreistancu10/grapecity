using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class ProcedureStateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "DocumentManagement",
                table: "Procedure");

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Procedure",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Procedure");

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "DocumentManagement",
                table: "Procedure",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
