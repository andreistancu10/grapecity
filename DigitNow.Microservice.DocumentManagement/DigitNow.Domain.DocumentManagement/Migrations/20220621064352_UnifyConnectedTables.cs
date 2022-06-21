using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class UnifyConnectedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocuments_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingDocuments",
                schema: "DocumentMangement",
                table: "OutgoingDocuments");

            migrationBuilder.RenameTable(
                name: "OutgoingDocuments",
                schema: "DocumentMangement",
                newName: "OutgoingDocument",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingDocuments_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                newName: "IX_OutgoingDocument_ContactDetailId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingDocument",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildDocumentId = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    IncomingDocumentId = table.Column<int>(type: "int", nullable: true),
                    OutgoingDocumentId = table.Column<long>(type: "bigint", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_OutgoingDocument_OutgoingDocumentId",
                        column: x => x.OutgoingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "OutgoingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_IncomingDocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "ConnectedDocument",
                column: "OutgoingDocumentId");

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
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowHistory_OutgoingDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingDocument",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.RenameTable(
                name: "OutgoingDocument",
                schema: "DocumentMangement",
                newName: "OutgoingDocuments",
                newSchema: "DocumentMangement");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                newName: "IX_OutgoingDocuments_ContactDetailId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingDocuments",
                schema: "DocumentMangement",
                table: "OutgoingDocuments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IncomingConnectedDocument",
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
                    table.PrimaryKey("PK_IncomingConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingConnectedDocument_IncomingDocument_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
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
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    OutgoingDocumentId = table.Column<long>(type: "bigint", nullable: true),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_IncomingConnectedDocument_IncomingDocumentId",
                schema: "DocumentMangement",
                table: "IncomingConnectedDocument",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingConnectedDocument_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "OutgoingConnectedDocument",
                column: "OutgoingDocumentId");

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
