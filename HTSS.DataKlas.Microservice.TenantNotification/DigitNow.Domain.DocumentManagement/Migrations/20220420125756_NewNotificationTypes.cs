using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class NewNotificationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { 39L, true, "CancelledRequesterInformativePlanningTeamDetail", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, null, true, false, null, null, "Cancelled Requester Informative PlanningTeamDetail", 4L, "Notification.PlanningTeamDetail.CancelledRequesterInformativePlanningTeamDetail" });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { 40L, true, "ApprovedRequesterInformativePlanningTeamDetail", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, null, true, false, null, null, "Approved Requester Informative PlanningTeamDetail", 2L, "Notification.PlanningTeamDetail.ApprovedRequesterInformativePlanningTeamDetail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 40L);
        }
    }
}
