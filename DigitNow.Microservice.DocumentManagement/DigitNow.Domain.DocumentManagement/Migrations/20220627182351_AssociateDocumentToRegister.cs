using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AssociateDocumentToRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentSpecialRegisterAssociations",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    SpecialRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    AssociationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSpecialRegisterAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSpecialRegisterAssociations_IncomingDocument_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSpecialRegisterAssociations_SpecialRegister_SpecialRegisterId",
                        column: x => x.SpecialRegisterId,
                        principalSchema: "DocumentMangement",
                        principalTable: "SpecialRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialRegisterAssociations_DocumentId",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                column: "DocumentId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSpecialRegisterAssociations",
                schema: "DocumentMangement");
        }
    }
}
