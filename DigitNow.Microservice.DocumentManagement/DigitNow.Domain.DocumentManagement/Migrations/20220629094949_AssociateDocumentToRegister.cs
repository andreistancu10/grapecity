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
                    AssociationId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId1 = table.Column<long>(type: "bigint", nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSpecialRegisterAssociations",
                schema: "DocumentMangement");
        }
    }
}
