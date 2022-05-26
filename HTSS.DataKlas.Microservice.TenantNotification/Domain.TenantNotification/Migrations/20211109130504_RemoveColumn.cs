using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class RemoveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelForSender",
                schema: "tenantnotification",
                table: "NotificationType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabelForSender",
                schema: "tenantnotification",
                table: "NotificationType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LabelForSender",
                value: "Notification.EmployeeRequest.PendingFlowSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "LabelForSender",
                value: "Notification.EmployeeRequest.PendingInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "LabelForSender",
                value: "Notification.EmployeeRequest.ApprovedInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 5L,
                column: "LabelForSender",
                value: "Notification.EmployeeRequest.CancelApprovedInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 7L,
                column: "LabelForSender",
                value: "Notification.EmployeeRequest.RejectedInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 9L,
                column: "LabelForSender",
                value: "Notification.LocationProgram.PendingInformativeForSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 10L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.PendingFlowSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 11L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.PendingInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 12L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.ApprovedFlowSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 13L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.ApprovedInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 15L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.CanceledBySourceInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 17L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.CanceledByTargetInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 19L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.RejectApprovedInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 21L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.RejectedByIneligibilityInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 23L,
                column: "LabelForSender",
                value: "Notification.CoverGapRequest.RejectedByExpiredTimeInformativeSender");

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 24L,
                column: "LabelForSender",
                value: "Notification.BusinessObjectives.PendingInformativeForSender");
        }
    }
}
