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
                name: "documentamangement");

            migrationBuilder.CreateTable(
                name: "ConnectedDocument",
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
                    table.PrimaryKey("PK_ConnectedDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetail",
                schema: "documentamangement",
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
                name: "Notification",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    NotificationTypeId = table.Column<long>(type: "bigint", nullable: false),
                    NotificationStatusId = table.Column<long>(type: "bigint", nullable: false),
                    FromUserId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    EntityTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    ReactiveSettings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeenOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatus",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocument",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    IsGDPRAgreed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocument_ContactDetail_ContactDetailId",
                        column: x => x.ContactDetailId,
                        principalSchema: "documentamangement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotificationType",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsInformative = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    NotificationStatusId = table.Column<long>(type: "bigint", nullable: false),
                    EntityType = table.Column<long>(type: "bigint", maxLength: 256, nullable: false),
                    TranslationLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationType_NotificationStatus_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalSchema: "documentamangement",
                        principalTable: "NotificationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "NotificationTypeCoverGapExtension",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeCoverGapExtension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypeCoverGapExtension_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalSchema: "documentamangement",
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "documentamangement",
                table: "NotificationStatus",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1L, true, "Pending", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pending" },
                    { 2L, true, "Approved", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Approved" },
                    { 3L, true, "Rejected", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Rejected" },
                    { 4L, true, "Cancelled", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cancelled" }
                });

            migrationBuilder.InsertData(
                schema: "documentamangement",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[,]
                {
                    { 1L, true, "PendingRequesterInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Pending Requester Informative Employee Request", 1L, "Notification.EmployeeRequest.PendingInformativeRequester" },
                    { 2L, true, "PendingManagerReactiveEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, false, false, null, null, "Pending Manager Reactive Employee Request", 1L, "Notification.EmployeeRequest.PendingReactiveManager" },
                    { 3L, true, "CancelledRequesterInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Cancelled Requester Informative Employee Request", 4L, "Notification.EmployeeRequest.CancelledInformativeRequester" },
                    { 4L, true, "CancelledManagerInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Cancelled Manager Informative Employee Request", 4L, "Notification.EmployeeRequest.CancelledInformativeManager" },
                    { 5L, true, "ApprovedRequesterInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Approved Requester Informative Employee Request", 2L, "Notification.EmployeeRequest.ApprovedInformativeRequester" },
                    { 6L, true, "ApprovedManagerInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Approved Manager Informative Employee Request", 2L, "Notification.EmployeeRequest.ApprovedInformativeManager" },
                    { 7L, true, "RejectedRequesterInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Rejected Requester Informative Employee Request", 3L, "Notification.EmployeeRequest.RejectedInformativeRequester" },
                    { 8L, true, "RejectedManagerInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, null, true, false, null, null, "Rejected Manager Informative Employee Request", 3L, "Notification.EmployeeRequest.RejectedInformativeManager" },
                    { 9L, true, "ConfiguratorSchedulingTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Scheduling types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.SchedulingTypes" },
                    { 10L, true, "ConfiguratorSchedulingDetailsTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Scheduling details types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.SchedulingDetailsTypes" },
                    { 11L, true, "ConfiguratorEmployeeMobilityTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Employee mobility types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.MobilityTypes" },
                    { 12L, true, "PendingResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Resource Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequestedInformativeCoverGapRequest" },
                    { 13L, true, "PendingResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Resource Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequesterInformativeCoverGapRequest" },
                    { 14L, true, "PendingLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Location Manager Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequestedInformativeCoverGapRequest" },
                    { 15L, true, "PendingLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Location Manager Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequesterInformativeCoverGapRequest" },
                    { 16L, true, "PendingDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Division Manager Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 17L, true, "PendingDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Division Manager Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 18L, true, "PendingResourceRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Resource Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequestedReactiveCoverGapRequest" },
                    { 19L, true, "PendingLocationManagerRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Location Manager Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequestedReactiveCoverGapRequest" },
                    { 20L, true, "PendingDivisionManagerRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Division Manager Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequestedReactiveCoverGapRequest" },
                    { 21L, true, "ApprovedResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Resource Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedResourceRequestedInformativeCoverGapRequest" },
                    { 22L, true, "ApprovedResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Resource Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedResourceRequesterInformativeCoverGapRequest" },
                    { 23L, true, "ApprovedLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Location Manager Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedLocationManagerRequestedInformativeCoverGapRequest" },
                    { 24L, true, "ApprovedLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Location Manager Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedLocationManagerRequesterInformativeCoverGapRequest" },
                    { 25L, true, "ApprovedDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Division Manager Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 26L, true, "ApprovedDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Division Manager Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 27L, true, "RejectedResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Resource Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedResourceRequestedInformativeCoverGapRequest" },
                    { 28L, true, "RejectedResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Resource Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedResourceRequesterInformativeCoverGapRequest" },
                    { 29L, true, "RejectedLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Location Manager Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedLocationManagerRequestedInformativeCoverGapRequest" },
                    { 30L, true, "RejectedLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Location Manager Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedLocationManagerRequesterInformativeCoverGapRequest" },
                    { 31L, true, "RejectedDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Division Manager Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 32L, true, "RejectedDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Division Manager Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 33L, true, "CancelledResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Resource Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledResourceRequestedInformativeCoverGapRequest" },
                    { 34L, true, "CancelledResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Resource Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledResourceRequesterInformativeCoverGapRequest" },
                    { 35L, true, "CancelledLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Location Manager Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledLocationManagerRequestedInformativeCoverGapRequest" },
                    { 36L, true, "CancelledLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Location Manager Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledLocationManagerRequesterInformativeCoverGapRequest" },
                    { 37L, true, "CancelledDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Division Manager Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 38L, true, "CancelledDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Division Manager Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 39L, true, "CancelledRequesterInformativePlanningTeamDetail", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, null, true, false, null, null, "Cancelled Requester Informative PlanningTeamDetail", 4L, "Notification.PlanningTeamDetail.CancelledRequesterInformativePlanningTeamDetail" },
                    { 40L, true, "ApprovedRequesterInformativePlanningTeamDetail", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, null, true, false, null, null, "Approved Requester Informative PlanningTeamDetail", 2L, "Notification.PlanningTeamDetail.ApprovedRequesterInformativePlanningTeamDetail" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDocumentIncomingDocument_IncomingDocumentsId",
                schema: "documentamangement",
                table: "ConnectedDocumentIncomingDocument",
                column: "IncomingDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_ContactDetailId",
                schema: "documentamangement",
                table: "IncomingDocument",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatus_Code",
                schema: "documentamangement",
                table: "NotificationStatus",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_Code",
                schema: "documentamangement",
                table: "NotificationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_NotificationStatusId",
                schema: "documentamangement",
                table: "NotificationType",
                column: "NotificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeCoverGapExtension_NotificationTypeId",
                schema: "documentamangement",
                table: "NotificationTypeCoverGapExtension",
                column: "NotificationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDocumentIncomingDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "NotificationTypeCoverGapExtension",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "ConnectedDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "IncomingDocument",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "NotificationType",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "ContactDetail",
                schema: "documentamangement");

            migrationBuilder.DropTable(
                name: "NotificationStatus",
                schema: "documentamangement");
        }
    }
}
