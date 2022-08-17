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
                name: "FormFields",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldMappings",
                schema: "DocumentMangement",
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
                    table.PrimaryKey("PK_FormFieldMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldMappings_FormFields_FormFieldId",
                        column: x => x.FormFieldId,
                        principalSchema: "DocumentMangement",
                        principalTable: "FormFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormFieldMappings_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFillingLogs",
                schema: "DocumentMangement",
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
                        name: "FK_FormFillingLogs_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFieldValues",
                schema: "DocumentMangement",
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
                    table.PrimaryKey("PK_FormFieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFieldValues_FormFieldMappings_FormFieldMappingId",
                        column: x => x.FormFieldMappingId,
                        principalSchema: "DocumentMangement",
                        principalTable: "FormFieldMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormFieldValues_FormFillingLogs_FormFillingLogId",
                        column: x => x.FormFillingLogId,
                        principalSchema: "DocumentMangement",
                        principalTable: "FormFillingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldMappings_FormFieldId",
                schema: "DocumentMangement",
                table: "FormFieldMappings",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldMappings_FormId",
                schema: "DocumentMangement",
                table: "FormFieldMappings",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValues_FormFieldMappingId",
                schema: "DocumentMangement",
                table: "FormFieldValues",
                column: "FormFieldMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFieldValues_FormFillingLogId",
                schema: "DocumentMangement",
                table: "FormFieldValues",
                column: "FormFillingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFillingLogs_FormId",
                schema: "DocumentMangement",
                table: "FormFillingLogs",
                column: "FormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormFieldValues",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "FormFieldMappings",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "FormFillingLogs",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "FormFields",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "DocumentMangement");
        }
    }
}
