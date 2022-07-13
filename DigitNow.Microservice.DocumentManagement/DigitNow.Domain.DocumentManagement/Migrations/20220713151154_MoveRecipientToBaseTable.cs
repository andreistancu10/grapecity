using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class MoveRecipientToBaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "IncomingDocument");

            migrationBuilder.AddColumn<long>(
                name: "InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "Document",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "InternalDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowHistory_InternalDocument_InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "InternalDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "InternalDocument",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowHistory_InternalDocument_InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowHistory_InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "InternalDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "Document");

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
