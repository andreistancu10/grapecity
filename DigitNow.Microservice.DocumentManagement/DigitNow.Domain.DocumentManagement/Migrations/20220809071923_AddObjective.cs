using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AddObjective : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objective", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralObjectives",
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
                    table.PrimaryKey("PK_GeneralObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralObjectives_Objective_ObjectiveId",
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
                name: "SpecificObjectives",
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
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificObjectives_GeneralObjectives_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "GeneralObjectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SpecificObjectives_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralObjectives_ObjectiveId",
                schema: "DocumentMangement",
                table: "GeneralObjectives",
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
                name: "IX_SpecificObjectives_GeneralObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjectives_ObjectiveId",
                schema: "DocumentMangement",
                table: "SpecificObjectives",
                column: "ObjectiveId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "SpecificObjectives",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "BasicUploadedFile",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "GeneralObjectives",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "Objective",
                schema: "DocumentMangement");
        }
    }
}
