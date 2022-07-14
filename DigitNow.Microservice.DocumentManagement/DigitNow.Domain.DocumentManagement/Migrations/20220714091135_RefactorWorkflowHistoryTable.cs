using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorWorkflowHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "IncomingDocument");

            migrationBuilder.AlterColumn<int>(
                name: "Resolution",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "Resolution",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RegistrationNumber",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
