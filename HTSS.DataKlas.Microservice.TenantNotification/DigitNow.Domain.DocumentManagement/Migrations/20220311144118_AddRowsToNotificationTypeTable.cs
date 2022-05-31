using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class AddRowsToNotificationTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTypeCoverGapExtension",
                schema: "tenantnotification",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTypeId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeCoverGapExtension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypeCoverGapExtension_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalSchema: "tenantnotification",
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[,]
                {
                    { 12L, true, "PendingResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Resource Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequestedInformativeCoverGapRequest" },
                    { 36L, true, "CancelledLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Location Manager Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledLocationManagerRequesterInformativeCoverGapRequest" },
                    { 35L, true, "CancelledLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Location Manager Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledLocationManagerRequestedInformativeCoverGapRequest" },
                    { 34L, true, "CancelledResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Resource Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledResourceRequesterInformativeCoverGapRequest" },
                    { 33L, true, "CancelledResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Resource Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledResourceRequestedInformativeCoverGapRequest" },
                    { 32L, true, "RejectedDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Division Manager Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 31L, true, "RejectedDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Division Manager Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 30L, true, "RejectedLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Location Manager Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedLocationManagerRequesterInformativeCoverGapRequest" },
                    { 29L, true, "RejectedLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Location Manager Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedLocationManagerRequestedInformativeCoverGapRequest" },
                    { 28L, true, "RejectedResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Resource Requester Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedResourceRequesterInformativeCoverGapRequest" },
                    { 27L, true, "RejectedResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Rejected Resource Requested Informative CoverGap Request", 3L, "Notification.CoverGapRequest.RejectedResourceRequestedInformativeCoverGapRequest" },
                    { 26L, true, "ApprovedDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Division Manager Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 37L, true, "CancelledDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Division Manager Requested Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 25L, true, "ApprovedDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Division Manager Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 23L, true, "ApprovedLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Location Manager Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedLocationManagerRequestedInformativeCoverGapRequest" },
                    { 22L, true, "ApprovedResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Resource Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedResourceRequesterInformativeCoverGapRequest" },
                    { 21L, true, "ApprovedResourceRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Resource Requested Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedResourceRequestedInformativeCoverGapRequest" },
                    { 20L, true, "PendingDivisionManagerRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Division Manager Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequestedReactiveCoverGapRequest" },
                    { 19L, true, "PendingLocationManagerRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Location Manager Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequestedReactiveCoverGapRequest" },
                    { 18L, true, "PendingResourceRequestedReactiveCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, false, false, null, null, "Pending Resource Requested Reactive CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequestedReactiveCoverGapRequest" },
                    { 17L, true, "PendingDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Division Manager Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequesterInformativeCoverGapRequest" },
                    { 16L, true, "PendingDivisionManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Division Manager Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingDivisionManagerRequestedInformativeCoverGapRequest" },
                    { 15L, true, "PendingLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Location Manager Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequesterInformativeCoverGapRequest" },
                    { 14L, true, "PendingLocationManagerRequestedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Location Manager Requested Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingLocationManagerRequestedInformativeCoverGapRequest" },
                    { 13L, true, "PendingResourceRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Pending Resource Requester Informative CoverGap Request", 1L, "Notification.CoverGapRequest.PendingResourceRequesterInformativeCoverGapRequest" },
                    { 24L, true, "ApprovedLocationManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Approved Location Manager Requester Informative CoverGap Request", 2L, "Notification.CoverGapRequest.ApprovedLocationManagerRequesterInformativeCoverGapRequest" },
                    { 38L, true, "CancelledDivisionManagerRequesterInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, null, true, false, null, null, "Cancelled Division Manager Requester Informative CoverGap Request", 4L, "Notification.CoverGapRequest.CancelledDivisionManagerRequesterInformativeCoverGapRequest" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeCoverGapExtension_NotificationTypeId",
                schema: "tenantnotification",
                table: "NotificationTypeCoverGapExtension",
                column: "NotificationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTypeCoverGapExtension",
                schema: "tenantnotification");

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 38L);
        }
    }
}
