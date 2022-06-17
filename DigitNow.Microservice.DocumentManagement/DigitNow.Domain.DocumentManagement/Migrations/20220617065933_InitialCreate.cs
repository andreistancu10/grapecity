using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.AddColumn<long>(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IncomingConnectedDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildIncomingDocumentId = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    IncomingDocumentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingConnectedDocument_IncomingDocument_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocuments",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientTypeId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    ExternalNumberDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactDetailId = table.Column<int>(type: "int", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionPeriod = table.Column<double>(type: "float", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "DocumentMangement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OutgoingConnectedDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildOutgoingDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    OutgoingDocumentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingConnectedDocument_OutgoingDocuments_OutgoingDocumentId",
                        column: x => x.OutgoingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "OutgoingDocuments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "OutgoingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingConnectedDocument_IncomingDocumentId",
                schema: "DocumentMangement",
                table: "IncomingConnectedDocument",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingConnectedDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                column: "OutgoingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                column: "ContactDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "OutgoingDocumentId",
                principalSchema: "DocumentMangement",
                principalTable: "OutgoingDocuments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocuments_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropTable(
                name: "IncomingConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "OutgoingConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "OutgoingDocuments",
                schema: "DocumentMangement");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowHistory_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildIncomingDocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    IncomingDocumentId = table.Column<int>(type: "int", nullable: true),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_IncomingDocument_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_IncomingDocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                column: "IncomingDocumentId");
        }
    }
}
