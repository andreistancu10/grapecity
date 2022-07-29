using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorConnectedDocumentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildDocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                schema: "DocumentMangement",
                table: "ConnectedDocument");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                newName: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_DocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectedDocument_Document_DocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                column: "DocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectedDocument_Document_DocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument");

            migrationBuilder.DropIndex(
                name: "IX_ConnectedDocument_DocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                newName: "RegistrationNumber");

            migrationBuilder.AddColumn<long>(
                name: "ChildDocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
