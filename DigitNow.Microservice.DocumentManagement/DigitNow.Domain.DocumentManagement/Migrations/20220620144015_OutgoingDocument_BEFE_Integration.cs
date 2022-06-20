using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class OutgoingDocument_BEFE_Integration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingConnectedDocument_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocuments_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingDocuments",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientTypeId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

            migrationBuilder.RenameTable(
                name: "OutgoingDocuments",
                schema: "DocumentMangement",
                newName: "OutgoingDocument",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                schema: "DocumentMangement",
                table: "InternalDocument",
                newName: "CreationDate");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingDocuments_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                newName: "IX_OutgoingDocument_ContactDetailId");

            migrationBuilder.AlterColumn<int>(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentSummary",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingDocument",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingConnectedDocument_OutgoingDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                column: "OutgoingDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "OutgoingDocument",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId",
                principalSchema: "DocumentMangement",
                principalTable: "ContactDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "OutgoingDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "OutgoingDocument",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingConnectedDocument_OutgoingDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingDocument",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.RenameTable(
                name: "OutgoingDocument",
                schema: "DocumentMangement",
                newName: "OutgoingDocuments",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "InternalDocument",
                newName: "RegistrationDate");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                newName: "IX_OutgoingDocuments_ContactDetailId");

            migrationBuilder.AlterColumn<long>(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentSummary",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RecipientTypeId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingDocuments",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingConnectedDocument_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                column: "OutgoingDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "OutgoingDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocuments_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                column: "ContactDetailId",
                principalSchema: "DocumentMangement",
                principalTable: "ContactDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "OutgoingDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "OutgoingDocuments",
                principalColumn: "Id");
        }
    }
}
