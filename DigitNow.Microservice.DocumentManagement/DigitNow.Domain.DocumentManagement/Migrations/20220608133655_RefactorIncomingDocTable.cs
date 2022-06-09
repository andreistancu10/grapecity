using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorIncomingDocTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDocumentIncomingDocument",
                schema: "documentamangement");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNumber",
                schema: "documentamangement",
                table: "IncomingDocument",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "documentamangement",
                table: "IncomingDocument",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNumber",
                schema: "documentamangement",
                table: "ConnectedDocument",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ChildIncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument",
                column: "IncomingDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectedDocument_IncomingDocument_IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument",
                column: "IncomingDocumentId",
                principalSchema: "documentamangement",
                principalTable: "IncomingDocument",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectedDocument_IncomingDocument_IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument");

            migrationBuilder.DropIndex(
                name: "IX_ConnectedDocument_IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "documentamangement",
                table: "IncomingDocument");

            migrationBuilder.DropColumn(
                name: "ChildIncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument");

            migrationBuilder.DropColumn(
                name: "IncomingDocumentId",
                schema: "documentamangement",
                table: "ConnectedDocument");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                schema: "documentamangement",
                table: "IncomingDocument",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                schema: "documentamangement",
                table: "ConnectedDocument",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ConnectedDocumentIncomingDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    ConnectedDocumentsId = table.Column<int>(type: "int", nullable: false),
                    IncomingDocumentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDocumentIncomingDocument", x => new { x.ConnectedDocumentsId, x.IncomingDocumentsId });
                    table.ForeignKey(
                        name: "FK_ConnectedDocumentIncomingDocument_ConnectedDocument_ConnectedDocumentsId",
                        column: x => x.ConnectedDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "ConnectedDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectedDocumentIncomingDocument_IncomingDocument_IncomingDocumentsId",
                        column: x => x.IncomingDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocumentIncomingDocument_IncomingDocumentsId",
                schema: "documentamangement",
                table: "ConnectedDocumentIncomingDocument",
                column: "IncomingDocumentsId");
        }
    }
}
