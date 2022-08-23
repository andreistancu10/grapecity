using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class UploadedFileRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentUploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.RenameColumn(
                name: "DocumentCategoryId",
                schema: "DocumentManagement",
                table: "UploadedFile",
                newName: "UploadedFileMappingId");

            migrationBuilder.CreateTable(
                name: "UploadedFileMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    TargetId = table.Column<long>(type: "bigint", nullable: false),
                    TargetEntity = table.Column<int>(type: "int", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFileMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFileMapping_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadedFileMapping_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFileMappings",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileMappingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFileMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFileMappings_UploadedFileMapping_UploadedFileMappingId",
                        column: x => x.UploadedFileMappingId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFileMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFileMappings_UploadedFileMappingId",
                schema: "DocumentManagement",
                table: "DocumentFileMappings",
                column: "UploadedFileMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFileMapping_DocumentId",
                schema: "DocumentManagement",
                table: "UploadedFileMapping",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFileMapping_UploadedFileId",
                schema: "DocumentManagement",
                table: "UploadedFileMapping",
                column: "UploadedFileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentFileMappings",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "UploadedFileMapping",
                schema: "DocumentManagement");

            migrationBuilder.RenameColumn(
                name: "UploadedFileMappingId",
                schema: "DocumentManagement",
                table: "UploadedFile",
                newName: "DocumentCategoryId");

            migrationBuilder.CreateTable(
                name: "DocumentUploadedFile",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUploadedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentUploadedFile_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentUploadedFile_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentManagement",
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
                        name: "FK_ObjectiveUploadedFile_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveUploadedFile_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_DocumentId",
                schema: "DocumentManagement",
                table: "DocumentUploadedFile",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_UploadedFileId",
                schema: "DocumentManagement",
                table: "DocumentUploadedFile",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_ObjectiveId",
                schema: "DocumentManagement",
                table: "ObjectiveUploadedFile",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_UploadedFileId",
                schema: "DocumentManagement",
                table: "ObjectiveUploadedFile",
                column: "UploadedFileId");
        }
    }
}
