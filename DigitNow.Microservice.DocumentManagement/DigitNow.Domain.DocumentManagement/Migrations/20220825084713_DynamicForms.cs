using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class DynamicForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FormFillingLogs",
                schema: "DocumentMangement",
                newName: "FormFillingLogs",
                newSchema: "DocumentManagement");

            migrationBuilder.RenameTable(
                name: "FormFieldValue",
                schema: "DocumentMangement",
                newName: "FormFieldValue",
                newSchema: "DocumentManagement");

            migrationBuilder.RenameTable(
                name: "FormFieldMapping",
                schema: "DocumentMangement",
                newName: "FormFieldMapping",
                newSchema: "DocumentManagement");

            migrationBuilder.RenameTable(
                name: "FormField",
                schema: "DocumentMangement",
                newName: "FormField",
                newSchema: "DocumentManagement");

            migrationBuilder.RenameTable(
                name: "Form",
                schema: "DocumentMangement",
                newName: "Form",
                newSchema: "DocumentManagement");

            migrationBuilder.RenameColumn(
                name: "FieldType",
                schema: "DocumentManagement",
                table: "FormField",
                newName: "DynamicFieldType");

            migrationBuilder.UpdateData(
                schema: "DocumentManagement",
                table: "FormField",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DynamicFieldType",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "DocumentManagement",
                table: "FormField",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DynamicFieldType",
                value: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "FormFillingLogs",
                schema: "DocumentManagement",
                newName: "FormFillingLogs",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "FormFieldValue",
                schema: "DocumentManagement",
                newName: "FormFieldValue",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "FormFieldMapping",
                schema: "DocumentManagement",
                newName: "FormFieldMapping",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "FormField",
                schema: "DocumentManagement",
                newName: "FormField",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameTable(
                name: "Form",
                schema: "DocumentManagement",
                newName: "Form",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameColumn(
                name: "DynamicFieldType",
                schema: "DocumentMangement",
                table: "FormField",
                newName: "FieldType");

            migrationBuilder.UpdateData(
                schema: "DocumentMangement",
                table: "FormField",
                keyColumn: "Id",
                keyValue: 4L,
                column: "FieldType",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "DocumentMangement",
                table: "FormField",
                keyColumn: "Id",
                keyValue: 6L,
                column: "FieldType",
                value: 5);
        }
    }
}
