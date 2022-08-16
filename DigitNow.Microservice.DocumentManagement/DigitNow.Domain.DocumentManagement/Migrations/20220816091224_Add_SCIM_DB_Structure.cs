using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Add_SCIM_DB_Structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceDestinationDepartmentId",
                schema: "DocumentMangement",
                table: "Document",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BasicUploadedFile",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsolutePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicUploadedFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objective",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveType = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objective", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralObjective",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralObjective", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralObjective_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveUploadedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveUploadedFile_BasicUploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentMangement",
                        principalTable: "BasicUploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveUploadedFile_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecificObjective",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificObjective", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificObjective_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "GeneralObjective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificObjective_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_GeneralObjective_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjective",
                column: "ObjectiveId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_ObjectiveId",
                schema: "DocumentMangement",
                table: "ObjectiveUploadedFile",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_UploadedFileId",
                schema: "DocumentMangement",
                table: "ObjectiveUploadedFile",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjective_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjective_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjective",
                column: "ObjectiveId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjectiveFunctionary_SpecificObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectiveFunctionary",
                column: "SpecificObjectiveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "SpecificObjectiveFunctionary",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "BasicUploadedFile",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "SpecificObjective",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "GeneralObjective",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "Objective",
                schema: "DocumentMangement");

            migrationBuilder.DropColumn(
                name: "SourceDestinationDepartmentId",
                schema: "DocumentMangement",
                table: "Document");
        }
    }
}
