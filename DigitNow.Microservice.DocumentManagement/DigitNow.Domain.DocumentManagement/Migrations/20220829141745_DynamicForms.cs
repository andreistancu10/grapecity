﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class DynamicForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicForm",
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
                    table.PrimaryKey("PK_DynamicForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormField",
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
                    table.PrimaryKey("PK_DynamicFormField", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormFillingLogs",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DynamicFormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFillingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFillingLogs_DynamicForm_DynamicFormId",
                        column: x => x.DynamicFormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormFieldMapping",
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
                    DynamicFormId = table.Column<long>(type: "bigint", nullable: false),
                    DynamicFormFieldId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFieldMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldMapping_DynamicForm_DynamicFormId",
                        column: x => x.DynamicFormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldMapping_DynamicFormField_DynamicFormFieldId",
                        column: x => x.DynamicFormFieldId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormFieldValue",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicFormFillingLogId = table.Column<long>(type: "bigint", nullable: false),
                    DynamicFormFieldMappingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFieldValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                        column: x => x.DynamicFormFieldMappingId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormFieldMapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldValue_DynamicFormFillingLogs_DynamicFormFillingLogId",
                        column: x => x.DynamicFormFillingLogId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormFillingLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "DynamicForm",
                columns: new[] { "Id", "Context", "Description", "Label", "Name" },
                values: new object[,]
                {
                    { 1L, "", "description1", "label1", "Formular 1" },
                    { 2L, "", "description2", "label2", "Formular 2" }
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "DynamicFormField",
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
                table: "DynamicFormFieldMapping",
                columns: new[] { "Id", "Context", "DynamicFormFieldId", "DynamicFormId", "InitialValue", "Key", "Label", "Order", "Required" },
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
                name: "IX_DynamicForm_Name",
                schema: "DocumentManagement",
                table: "DynamicForm",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormField_Name",
                schema: "DocumentManagement",
                table: "DynamicFormField",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldMapping_DynamicFormFieldId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldMapping",
                column: "DynamicFormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldMapping_DynamicFormId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldMapping",
                column: "DynamicFormId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldValue_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFieldMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldValue_DynamicFormFillingLogId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFillingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFillingLogs_DynamicFormId",
                schema: "DocumentManagement",
                table: "DynamicFormFillingLogs",
                column: "DynamicFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicFormFieldValue",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormFieldMapping",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormFillingLogs",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormField",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicForm",
                schema: "DocumentManagement");
        }
    }
}