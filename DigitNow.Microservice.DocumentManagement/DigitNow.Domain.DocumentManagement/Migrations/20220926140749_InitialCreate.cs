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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                name: "DeliveryDetail",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeliveryMode = table.Column<int>(type: "int", nullable: false),
                    DirectShipping = table.Column<int>(type: "int", nullable: false),
                    Post = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetail", x => x.Id);
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                name: "DynamicForm",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormField",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DynamicFieldType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormField", x => x.Id);
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    UploadedFileMappingId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AbsolutePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedName = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAction",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    ResposibleId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAction_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_IncomingDocument_DeliveryDetail_DeliveryDetailsId",
                        column: x => x.DeliveryDetailsId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DeliveryDetail",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_OutgoingDocument_DeliveryDetail_DeliveryDetailsId",
                        column: x => x.DeliveryDetailsId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DeliveryDetail",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                name: "DynamicFormFillingLog",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DynamicFormId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFillingLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFillingLog_DynamicForm_DynamicFormId",
                        column: x => x.DynamicFormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicForm",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormFieldMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    InitialValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicFormId = table.Column<long>(type: "bigint", nullable: false),
                    DynamicFormFieldId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFieldMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldMapping_DynamicForm_DynamicFormId",
                        column: x => x.DynamicFormId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldMapping_DynamicFormField_DynamicFormFieldId",
                        column: x => x.DynamicFormFieldId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormField",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                name: "SpecialRegisterMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRegisterMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRegisterMapping_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialRegisterMapping_SpecialRegister_SpecialRegisterId",
                        column: x => x.SpecialRegisterId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecialRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFileMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    TargetId = table.Column<long>(type: "bigint", nullable: false),
                    TargetEntity = table.Column<int>(type: "int", nullable: false),
                    UploadedFileId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFileMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFileMapping_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadedFileMapping_UploadedFile_UploadedFileId",
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
                name: "DynamicFormFieldValue",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicFormFillingLogId = table.Column<long>(type: "bigint", nullable: false),
                    DynamicFormFieldMappingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicFormFieldValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldValue_DynamicFormFieldMapping_DynamicFormFieldMappingId",
                        column: x => x.DynamicFormFieldMappingId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormFieldMapping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DynamicFormFieldValue_DynamicFormFillingLog_DynamicFormFillingLogId",
                        column: x => x.DynamicFormFillingLogId,
                        principalSchema: "DocumentManagement",
                        principalTable: "DynamicFormFillingLog",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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
                name: "DocumentFileMapping",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedFileMappingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFileMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFileMapping_UploadedFileMapping_UploadedFileMappingId",
                        column: x => x.UploadedFileMappingId,
                        principalSchema: "DocumentManagement",
                        principalTable: "UploadedFileMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationMotive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityFunctionary_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionFunctionary",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ActionId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionaryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionFunctionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionFunctionary_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    GeneralObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    SpecificObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    RiskCauses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskConsequences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbabilityOfApparitionEstimation = table.Column<int>(type: "int", nullable: false),
                    ImpactOfObjectivesEstimation = table.Column<int>(type: "int", nullable: false),
                    RiskExposureEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadOfDepartmentDecision = table.Column<int>(type: "int", nullable: false),
                    HeadOfDepartmentAssignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdoptedStrategy = table.Column<int>(type: "int", nullable: false),
                    AdoptedStrategyAssignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrategyDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilizedDocumentation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risk_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_GeneralObjective_GeneralObjectiveId",
                        column: x => x.GeneralObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "GeneralObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_SpecificObjective_SpecificObjectiveId",
                        column: x => x.SpecificObjectiveId,
                        principalSchema: "DocumentManagement",
                        principalTable: "SpecificObjective",
                        principalColumn: "ObjectiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlAction",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RiskId = table.Column<long>(type: "bigint", nullable: false),
                    ControlMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlAction_Risk_RiskId",
                        column: x => x.RiskId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskTrackingReport",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RiskId = table.Column<long>(type: "bigint", nullable: false),
                    ControlMeasuresImplementationState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbabilityOfApparitionEstimation = table.Column<int>(type: "int", nullable: false),
                    ImpactOfObjectivesEstimation = table.Column<int>(type: "int", nullable: false),
                    RiskExposureEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTrackingReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskTrackingReport_Risk_RiskId",
                        column: x => x.RiskId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskActionProposal",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RiskTrackingReportId = table.Column<long>(type: "bigint", nullable: false),
                    ProposedAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskTrackingReportDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskActionProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskActionProposal_RiskTrackingReport_RiskTrackingReportId",
                        column: x => x.RiskTrackingReportId,
                        principalSchema: "DocumentManagement",
                        principalTable: "RiskTrackingReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "DynamicForm",
                columns: new[] { "Id", "Context", "Description", "Label", "Name" },
                values: new object[,]
                {
                    { 1L, "", "description1", "label1", "Formular 1" },
                    { 2L, "", "description2", "label2", "Formular 2" }
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "DynamicFormField",
                columns: new[] { "Id", "Context", "DynamicFieldType", "Name" },
                values: new object[,]
                {
                    { 1L, "", 0, "Input" },
                    { 2L, "", 1, "Number" },
                    { 3L, "", 2, "Date" },
                    { 4L, "", 5, "CountryDropdown" },
                    { 5L, "", 4, "DistrictDropdown" },
                    { 6L, "", 3, "CityDropdown" }
                });

            migrationBuilder.InsertData(
                schema: "DocumentManagement",
                table: "DynamicFormFieldMapping",
                columns: new[] { "Id", "Context", "DynamicFormFieldId", "DynamicFormId", "InitialValue", "Key", "Label", "Order", "Required" },
                values: new object[,]
                {
                    { 1L, "", 1L, 1L, "", "lastName", "Nume", 1, true },
                    { 2L, "", 1L, 1L, "", "firstName", "Prenume", 2, true },
                    { 3L, "", 2L, 1L, "7", "resolutionPeriod", "Termen Solutionare", 3, false },
                    { 4L, "", 3L, 1L, "", "createdDate", "Data Creare", 4, true },
                    { 5L, "", 1L, 2L, "", "observations", "Observatii", 1, true },
                    { 6L, "", 4L, 2L, "161", "countryId", "Tara", 2, false },
                    { 7L, "", 6L, 2L, "", "cityId", "Oras", 3, false },
                    { 8L, "", 5L, 2L, "", "districtId", "Judet", 4, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ActivityId",
                schema: "DocumentManagement",
                table: "Action",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFunctionary_ActionId",
                schema: "DocumentManagement",
                table: "ActionFunctionary",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "Activity",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "Activity",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityFunctionary_ActivityId",
                schema: "DocumentManagement",
                table: "ActivityFunctionary",
                column: "ActivityId");

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
                name: "IX_DocumentAction_DocumentId",
                schema: "DocumentManagement",
                table: "DocumentAction",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFileMapping_UploadedFileMappingId",
                schema: "DocumentManagement",
                table: "DocumentFileMapping",
                column: "UploadedFileMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicForm_Name",
                schema: "DocumentManagement",
                table: "DynamicForm",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormField_Name",
                schema: "DocumentManagement",
                table: "DynamicFormField",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldMapping_DynamicFormFieldId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldMapping",
                column: "DynamicFormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldMapping_DynamicFormId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldMapping",
                column: "DynamicFormId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldValue_DynamicFormFieldMappingId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFieldMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFieldValue_DynamicFormFillingLogId",
                schema: "DocumentManagement",
                table: "DynamicFormFieldValue",
                column: "DynamicFormFillingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormFillingLog_DynamicFormId",
                schema: "DocumentManagement",
                table: "DynamicFormFillingLog",
                column: "DynamicFormId");

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
                name: "IX_Risk_ActionId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ActivityId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_GeneralObjectiveId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "GeneralObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_SpecificObjectiveId",
                schema: "DocumentManagement",
                table: "Risk",
                column: "SpecificObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskActionProposal_RiskTrackingReportId",
                schema: "DocumentManagement",
                table: "RiskActionProposal",
                column: "RiskTrackingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlAction_RiskId",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskTrackingReport_RiskId",
                schema: "DocumentManagement",
                table: "RiskTrackingReport",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegister_DocumentCategoryId",
                schema: "DocumentManagement",
                table: "SpecialRegister",
                column: "DocumentCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterMapping_DocumentId",
                schema: "DocumentManagement",
                table: "SpecialRegisterMapping",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegisterMapping_SpecialRegisterId",
                schema: "DocumentManagement",
                table: "SpecialRegisterMapping",
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
                name: "IX_UploadedFileMapping_DocumentId",
                schema: "DocumentManagement",
                table: "UploadedFileMapping",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFileMapping_UploadedFileId",
                schema: "DocumentManagement",
                table: "UploadedFileMapping",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistoryLog_DocumentId",
                schema: "DocumentManagement",
                table: "WorkflowHistoryLog",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ActivityFunctionary",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DocumentAction",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DocumentFileMapping",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DocumentResolution",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormFieldValue",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "InternalDocument",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "RiskActionProposal",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "RiskControlAction",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecialRegisterMapping",
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
                name: "UploadedFileMapping",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormFieldMapping",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormFillingLog",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "RiskTrackingReport",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecialRegister",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DeliveryDetail",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "UploadedFile",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicFormField",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "DynamicForm",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Risk",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Activity",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "SpecificObjective",
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
