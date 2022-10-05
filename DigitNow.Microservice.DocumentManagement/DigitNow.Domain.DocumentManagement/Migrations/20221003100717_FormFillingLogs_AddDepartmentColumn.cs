using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class FormFillingLogs_AddDepartmentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DestinationDepartmentId",
                schema: "DocumentManagement",
                table: "DynamicFormFillingLog",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationDepartmentId",
                schema: "DocumentManagement",
                table: "DynamicFormFillingLog");
        }
    }
}
