using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class RemoveCancelledApprovedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Code", "Name" },
                values: new object[] { "Cancelled", "Cancelled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Code", "Name" },
                values: new object[] { "CancelApproved", "CancelApproved" });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[] { 5L, true, "Cancelled", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cancelled" });
        }
    }
}
