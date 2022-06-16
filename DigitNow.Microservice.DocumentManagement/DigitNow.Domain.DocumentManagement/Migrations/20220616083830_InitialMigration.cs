using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DocumentMangement");

            migrationBuilder.CreateTable(
                name: "ContactDetail",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CountyId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Building = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entrance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    InternalDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DeadlineDaysNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverDepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationNumberCounter",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationNumberCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputChannelId = table.Column<int>(type: "int", nullable: false),
                    IssuerTypeId = table.Column<int>(type: "int", nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    ExternalNumberDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactDetailId = table.Column<int>(type: "int", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionPeriod = table.Column<double>(type: "float", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true),
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocument_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "DocumentMangement",
                        principalTable: "ContactDetail",
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

            migrationBuilder.CreateTable(
                name: "WorkflowHistory",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeclineReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpinionRequestedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncomingDocumentId = table.Column<int>(type: "int", nullable: true),
                    OutgoingDocumentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowHistory_IncomingDocument_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowHistory_OutgoingDocuments_OutgoingDocumentId",
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
                name: "IX_IncomingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                column: "ContactDetailId");

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

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_IncomingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_OutgoingDocumentId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                column: "OutgoingDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "InternalDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "OutgoingConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "RegistrationNumberCounter",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "WorkflowHistory",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "IncomingDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "OutgoingDocuments",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "DocumentMangement");
        }
    }
}
