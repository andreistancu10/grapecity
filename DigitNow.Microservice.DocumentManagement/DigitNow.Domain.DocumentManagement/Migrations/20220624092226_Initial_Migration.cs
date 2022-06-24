using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Initial_Migration : Migration
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
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
                name: "Document",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentResolution",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    ResolutionType = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentResolution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationNumberCounters",
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
                    table.PrimaryKey("PK_RegistrationNumberCounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    InputChannelId = table.Column<int>(type: "int", nullable: false),
                    IssuerTypeId = table.Column<int>(type: "int", nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    ExternalNumberDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactDetailId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionPeriod = table.Column<double>(type: "float", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true),
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_IncomingDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternalDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    InternalDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DeadlineDaysNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverDepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificationNumber = table.Column<long>(type: "bigint", nullable: false),
                    ContactDetailId = table.Column<int>(type: "int", nullable: false),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactDetailId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocument_ContactDetail_ContactDetailId1",
                        column: x => x.ContactDetailId1,
                        principalSchema: "DocumentMangement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutgoingDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ChildDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationNumber = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_IncomingDocument_Id",
                        column: x => x.Id,
                        principalSchema: "DocumentMangement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_OutgoingDocument_Id",
                        column: x => x.Id,
                        principalSchema: "DocumentMangement",
                        principalTable: "OutgoingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowHistory",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
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
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncomingDocumentId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_WorkflowHistory_OutgoingDocument_OutgoingDocumentId",
                        column: x => x.OutgoingDocumentId,
                        principalSchema: "DocumentMangement",
                        principalTable: "OutgoingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_DocumentId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalDocument_DocumentId",
                schema: "DocumentMangement",
                table: "InternalDocument",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId1");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_DocumentId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "DocumentId",
                unique: true);

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
                name: "ConnectedDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "DocumentResolution",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "InternalDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "RegistrationNumberCounters",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "WorkflowHistory",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "IncomingDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "OutgoingDocument",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "DocumentMangement");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "DocumentMangement");
        }
    }
}
