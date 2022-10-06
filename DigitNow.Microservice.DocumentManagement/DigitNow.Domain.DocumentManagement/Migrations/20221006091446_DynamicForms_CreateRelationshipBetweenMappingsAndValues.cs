using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class DynamicForms_CreateRelationshipBetweenMappingsAndValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFieldMappingId",
                principalSchema: "DocumentManagement",
                principalTable: "DynamicFormFieldMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFieldMappingId",
                principalSchema: "DocumentManagement",
                principalTable: "DynamicFormFieldMapping",
                principalColumn: "Id");
        }
    }
}
