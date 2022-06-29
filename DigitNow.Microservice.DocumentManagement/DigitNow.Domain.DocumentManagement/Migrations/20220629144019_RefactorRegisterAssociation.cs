using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorRegisterAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSpecialRegisterAssociations",
                schema: "DocumentMangement");

            migrationBuilder.CreateTable(
                name: "SpecialRegisterAssociations",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    SpecialRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRegisterAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRegisterAssociations_IncomingDocument_DocumentId1",
                        column: x => x.DocumentId1,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecialRegisterAssociations_SpecialRegister_SpecialRegisterId",
                        column: x => x.SpecialRegisterId,
                        principalSchema: "DocumentMangement",
                        principalTable: "SpecialRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterAssociations_DocumentId1",
                schema: "DocumentMangement",
                table: "SpecialRegisterAssociations",
                column: "DocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterAssociations_SpecialRegisterId",
                schema: "DocumentMangement",
                table: "SpecialRegisterAssociations",
                column: "SpecialRegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialRegisterAssociations",
                schema: "DocumentMangement");

            migrationBuilder.CreateTable(
                name: "DocumentSpecialRegisterAssociations",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId1 = table.Column<long>(type: "bigint", nullable: true),
                    SpecialRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    AssociationId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSpecialRegisterAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSpecialRegisterAssociations_IncomingDocument_DocumentId1",
                        column: x => x.DocumentId1,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentSpecialRegisterAssociations_SpecialRegister_SpecialRegisterId",
                        column: x => x.SpecialRegisterId,
                        principalSchema: "DocumentMangement",
                        principalTable: "SpecialRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialRegisterAssociations_DocumentId1",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                column: "DocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialRegisterAssociations_Id",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialRegisterAssociations_SpecialRegisterId",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                column: "SpecialRegisterId");
        }
    }
}
