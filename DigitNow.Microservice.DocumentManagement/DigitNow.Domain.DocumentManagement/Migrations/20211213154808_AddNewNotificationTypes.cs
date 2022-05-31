using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class AddNewNotificationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { 9L, true, "ConfiguratorSchedulingTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Scheduling types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.SchedulingTypes" });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { 10L, true, "ConfiguratorSchedulingDetailsTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Scheduling details types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.SchedulingDetailsTypes" });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { 11L, true, "ConfiguratorEmployeeMobilityTypesNotification", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, null, true, false, null, null, "Employee mobility types settings changed in Configurator", 2L, "Notification.ConfiguratorNotification.MobilityTypes" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 11L);
        }
    }
}
