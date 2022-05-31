using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class NewNotificationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[] { 5L, true, "Cancelled", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cancelled" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Code",
                value: "CancelApprovedInformativeEmployeeRequest");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Code",
                value: "CancelApprovedFlowEmployeeRequest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Code",
                value: "CancelApprovedinformativeEmployeeRequest");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Code",
                value: "CancelApprovedflowEmployeeRequest");
        }
    }
}
