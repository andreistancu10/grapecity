using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class OutgoingDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDocumentIncomingDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "documentamangement");

            migrationBuilder.CreateTable(
                name: "IncomingConnectedDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingConnectedDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingConnectedDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingConnectedDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocuments",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputChannelId = table.Column<int>(type: "int", nullable: false),
                    RecipientTypeId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    ExternalNumberDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactDetailId = table.Column<int>(type: "int", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionPeriod = table.Column<double>(type: "float", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "documentamangement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IncomingConnectedDocumentIncomingDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    ConnectedDocumentsId = table.Column<int>(type: "int", nullable: false),
                    IncomingDocumentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingConnectedDocumentIncomingDocument", x => new { x.ConnectedDocumentsId, x.IncomingDocumentsId });
                    table.ForeignKey(
                        name: "FK_IncomingConnectedDocumentIncomingDocument_IncomingConnectedDocument_ConnectedDocumentsId",
                        column: x => x.ConnectedDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "IncomingConnectedDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingConnectedDocumentIncomingDocument_IncomingDocument_IncomingDocumentsId",
                        column: x => x.IncomingDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingConnectedDocumentOutgoingDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    ConnectedDocumentsId = table.Column<int>(type: "int", nullable: false),
                    OutgoingDocumentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingConnectedDocumentOutgoingDocument", x => new { x.ConnectedDocumentsId, x.OutgoingDocumentsId });
                    table.ForeignKey(
                        name: "FK_OutgoingConnectedDocumentOutgoingDocument_OutgoingConnectedDocument_ConnectedDocumentsId",
                        column: x => x.ConnectedDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "OutgoingConnectedDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingConnectedDocumentOutgoingDocument_OutgoingDocuments_OutgoingDocumentsId",
                        column: x => x.OutgoingDocumentsId,
                        principalSchema: "documentamangement",
                        principalTable: "OutgoingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingConnectedDocumentIncomingDocument_IncomingDocumentsId",
                schema: "documentamangement",
                table: "IncomingConnectedDocumentIncomingDocument",
                column: "IncomingDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingConnectedDocumentOutgoingDocument_OutgoingDocumentsId",
                schema: "documentamangement",
                table: "OutgoingConnectedDocumentOutgoingDocument",
                column: "OutgoingDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_ContactDetailId",
                schema: "documentamangement",
                table: "OutgoingDocuments",
                column: "ContactDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingConnectedDocumentIncomingDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "OutgoingConnectedDocumentOutgoingDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "IncomingConnectedDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "OutgoingConnectedDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "OutgoingDocuments",
                schema: "documentamangement");

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDocument", x => x.Id);
                });

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
