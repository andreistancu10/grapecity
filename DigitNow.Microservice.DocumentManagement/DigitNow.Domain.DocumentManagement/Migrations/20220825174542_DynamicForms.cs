using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class DynamicForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Form",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormField",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DynamicFieldType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormField", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormFillingLogs",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    FormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFillingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFillingLogs_Form_FormId",
                        column: x => x.FormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    InitialValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormId = table.Column<long>(type: "bigint", nullable: false),
                    FormFieldId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFieldMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldMapping_Form_FormId",
                        column: x => x.FormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormFieldMapping_FormField_FormFieldId",
                        column: x => x.FormFieldId,
                        principalSchema: "DocumentManagement",
                        principalTable: "FormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldValue",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormFillingLogId = table.Column<long>(type: "bigint", nullable: false),
                    FormFieldMappingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFieldValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldValue_FormFieldMapping_FormFieldMappingId",
                        column: x => x.FormFieldMappingId,
                        principalSchema: "DocumentManagement",
                        principalTable: "FormFieldMapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormFieldValue_FormFillingLogs_FormFillingLogId",
                        column: x => x.FormFillingLogId,
                        principalSchema: "DocumentManagement",
                        principalTable: "FormFillingLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "Form",
                columns: new[] { "Id", "Context", "Description", "Label", "Name" },
                values: new object[,]
                {
                    { 1L, "", "description1", "label1", "Formular 1" },
                    { 2L, "", "description2", "label2", "Formular 2" }
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "FormField",
                columns: new[] { "Id", "Context", "DynamicFieldType", "Name" },
                values: new object[,]
                {
                    { 1L, "", 0, "Input" },
                    { 2L, "", 1, "Number" },
                    { 3L, "", 2, "Date" },
                    { 4L, "", 5, "CountryDropdown" },
                    { 5L, "", 4, "DistrictDropdown" },
                    { 6L, "", 3, "CityDropdown" }
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "FormFieldMapping",
                columns: new[] { "Id", "Context", "FormFieldId", "FormId", "InitialValue", "Key", "Label", "Order", "Required" },
                values: new object[,]
                {
                    { 1L, "", 1L, 1L, "", "lastName", "Nume", 1, true },
                    { 2L, "", 1L, 1L, "", "firstName", "Prenume", 2, true },
                    { 3L, "", 2L, 1L, "7", "resolutionPeriod", "Termen Solutionare", 3, false },
                    { 4L, "", 3L, 1L, "", "createdDate", "Data Creare", 4, true },
                    { 5L, "", 1L, 2L, "", "observations", "Observatii", 1, true },
                    { 6L, "", 4L, 2L, "161", "countryId", "Tara", 2, false },
                    { 7L, "", 6L, 2L, "", "cityId", "Oras", 3, false },
                    { 8L, "", 5L, 2L, "", "districtId", "Judet", 4, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Form_Name",
                schema: "DocumentManagement",
                table: "Form",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormField_Name",
                schema: "DocumentManagement",
                table: "FormField",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldMapping_FormFieldId",
                schema: "DocumentManagement",
                table: "FormFieldMapping",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldMapping_FormId",
                schema: "DocumentManagement",
                table: "FormFieldMapping",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValue_FormFieldMappingId",
                schema: "DocumentManagement",
                table: "FormFieldValue",
                column: "FormFieldMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValue_FormFillingLogId",
                schema: "DocumentManagement",
                table: "FormFieldValue",
                column: "FormFillingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFillingLogs_FormId",
                schema: "DocumentManagement",
                table: "FormFillingLogs",
                column: "FormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormFieldValue",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "FormFieldMapping",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "FormFillingLogs",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "FormField",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Form",
                schema: "DocumentManagement");
        }
    }
}
