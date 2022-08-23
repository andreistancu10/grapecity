using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DocumentManagement");

            migrationBuilder.CreateTable(
                name: "ContactDetail",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
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
                name: "DeliveryDetails",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryMode = table.Column<int>(type: "int", nullable: false),
                    DirectShipping = table.Column<int>(type: "int", nullable: false),
                    Post = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecipientId = table.Column<long>(type: "bigint", nullable: true),
                    DestinationDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    SourceDestinationDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentResolution",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    ResolutionType = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentResolution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objective",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveType = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objective", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialRegister",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFile",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsolutePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocument",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    InputChannelId = table.Column<int>(type: "int", nullable: false),
                    IssuerTypeId = table.Column<int>(type: "int", nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    ExternalNumberDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionPeriod = table.Column<double>(type: "float", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true),
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: true),
                    ContactDetailId = table.Column<long>(type: "bigint", nullable: true),
                    DeliveryDetailsId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocument_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "DocumentManagement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IncomingDocument_DeliveryDetails_DeliveryDetailsId",
                        column: x => x.DeliveryDetailsId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DeliveryDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IncomingDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternalDocument",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    SourceDepartmentId = table.Column<int>(type: "int", nullable: false),
                    InternalDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DeadlineDaysNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocument",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetailId = table.Column<long>(type: "bigint", nullable: false),
                    ContentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryDetailsId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "DocumentManagement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingDocument_DeliveryDetails_DeliveryDetailsId",
                        column: x => x.DeliveryDetailsId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DeliveryDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutgoingDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowHistoryLog",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentStatus = table.Column<int>(type: "int", nullable: false),
                    DestinationDepartmentId = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<long>(type: "bigint", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeclineReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resolution = table.Column<int>(type: "int", nullable: true),
                    OpinionRequestedUntil = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowHistoryLog_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralObjective",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralObjective", x => x.Id);
                    table.UniqueConstraint("AK_GeneralObjective_ObjectiveId", x => x.ObjectiveId);
                    table.ForeignKey(
                        name: "FK_GeneralObjective_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialRegisterMappings",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRegisterMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRegisterMappings_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialRegisterMappings_SpecialRegister_SpecialRegisterId",
                        column: x => x.SpecialRegisterId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecialRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentUploadedFile",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUploadedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentUploadedFile_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentUploadedFile_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveUploadedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveUploadedFile_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveUploadedFile_UploadedFile_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    IncomingDocumentId = table.Column<long>(type: "bigint", nullable: true),
                    OutgoingDocumentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_IncomingDocument_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "IncomingDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConnectedDocument_OutgoingDocument_OutgoingDocumentId",
                        column: x => x.OutgoingDocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "OutgoingDocument",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecificObjective",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificObjective", x => x.Id);
                    table.UniqueConstraint("AK_SpecificObjective_ObjectiveId", x => x.ObjectiveId);
                    table.ForeignKey(
                        name: "FK_SpecificObjective_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificObjective_Objective_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Objective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecificObjectiveFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificObjectiveFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificObjectiveFunctionary_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_DocumentId",
                schema: "DocumentManagement",
                table: "ConnectedDocument",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_IncomingDocumentId",
                schema: "DocumentManagement",
                table: "ConnectedDocument",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocument_OutgoingDocumentId",
                schema: "DocumentManagement",
                table: "ConnectedDocument",
                column: "OutgoingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_DocumentId",
                schema: "DocumentManagement",
                table: "DocumentUploadedFile",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUploadedFile_UploadedFileId",
                schema: "DocumentManagement",
                table: "DocumentUploadedFile",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_ContactDetailId",
                schema: "DocumentManagement",
                table: "IncomingDocument",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_DeliveryDetailsId",
                schema: "DocumentManagement",
                table: "IncomingDocument",
                column: "DeliveryDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_DocumentId",
                schema: "DocumentManagement",
                table: "IncomingDocument",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalDocument_DocumentId",
                schema: "DocumentManagement",
                table: "InternalDocument",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_ObjectiveId",
                schema: "DocumentManagement",
                table: "ObjectiveUploadedFile",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveUploadedFile_UploadedFileId",
                schema: "DocumentManagement",
                table: "ObjectiveUploadedFile",
                column: "UploadedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentManagement",
                table: "OutgoingDocument",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_DeliveryDetailsId",
                schema: "DocumentManagement",
                table: "OutgoingDocument",
                column: "DeliveryDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_DocumentId",
                schema: "DocumentManagement",
                table: "OutgoingDocument",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegister_DocumentCategoryId",
                schema: "DocumentManagement",
                table: "SpecialRegister",
                column: "DocumentCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterMappings_DocumentId",
                schema: "DocumentManagement",
                table: "SpecialRegisterMappings",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterMappings_SpecialRegisterId",
                schema: "DocumentManagement",
                table: "SpecialRegisterMappings",
                column: "SpecialRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjective_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "SpecificObjective",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificObjectiveFunctionary_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "SpecificObjectiveFunctionary",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistoryLog_DocumentId",
                schema: "DocumentManagement",
                table: "WorkflowHistoryLog",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DocumentResolution",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DocumentUploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "InternalDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ObjectiveUploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecialRegisterMappings",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecificObjectiveFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "WorkflowHistoryLog",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "IncomingDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "OutgoingDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "UploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecialRegister",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecificObjective",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DeliveryDetails",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "GeneralObjective",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Objective",
                schema: "DocumentManagement");
        }
    }
}
