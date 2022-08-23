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
                schema: "DocumentMangement");

            migrationBuilder.RenameColumn(
                name: "DocumentCategoryId",
                schema: "DocumentMangement",
                table: "UploadedFile",
                newName: "UploadedFileMappingId");

            migrationBuilder.CreateTable(
                name: "DocumentFileMappings",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFileMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFileMappings_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentMangement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFileMapping",
                schema: "DocumentMangement",
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
                        principalSchema: "DocumentMangement",
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadedFileMapping_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentMangement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFileMappings_UploadedFileId",
                schema: "DocumentMangement",
                table: "DocumentFileMappings",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFileMapping_DocumentId",
                schema: "DocumentMangement",
                table: "UploadedFileMapping",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFileMapping_UploadedFileId",
                schema: "DocumentMangement",
                table: "UploadedFileMapping",
                column: "UploadedFileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentFileMappings",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "UploadedFileMapping",
                schema: "DocumentMangement");

            migrationBuilder.RenameColumn(
                name: "UploadedFileMappingId",
                schema: "DocumentMangement",
                table: "UploadedFile",
                newName: "DocumentCategoryId");

            migrationBuilder.CreateTable(
                name: "DocumentUploadedFile",
                schema: "DocumentMangement",
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
                        principalSchema: "DocumentMangement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentUploadedFile_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentMangement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_DocumentId",
                schema: "DocumentMangement",
                table: "DocumentUploadedFile",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_UploadedFileId",
                schema: "DocumentMangement",
                table: "DocumentUploadedFile",
                column: "UploadedFileId");
        }
    }
}
