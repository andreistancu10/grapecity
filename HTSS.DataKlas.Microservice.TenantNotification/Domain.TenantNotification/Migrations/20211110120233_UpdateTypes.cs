using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class UpdateTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.IsSqlServer())
            {
                migrationBuilder.Sql("delete from [tenantnotification].[Notification]");
            }

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

            migrationBuilder.DropColumn(
                name: "LabelForReceiver",
                schema: "tenantnotification",
                table: "NotificationType");

            migrationBuilder.DropColumn(
                name: "EntityType",
                schema: "tenantnotification",
                table: "NotificationType");

            migrationBuilder.AddColumn<long>(
                name: "EntityType",
                schema: "tenantnotification",
                table: "NotificationType",
                defaultValue: 0,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslationLabel",
                schema: "tenantnotification",
                table: "NotificationType",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Code", "EntityType", "IsInformative", "Name", "TranslationLabel" },
                values: new object[] { "PendingRequesterInformativeEmployeeRequest", 1L, true, "Pending Requester Informative Employee Request", "Notification.EmployeeRequest.PendingInformativeRequester" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Code", "EntityType", "IsInformative", "Name", "TranslationLabel" },
                values: new object[] { "PendingManagerReactiveEmployeeRequest", 1L, false, "Pending Manager Reactive Employee Request", "Notification.EmployeeRequest.PendingReactiveManager" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Code", "EntityType", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { "CancelledRequesterInformativeEmployeeRequest", 1L, "Cancelled Requester Informative Employee Request", 4L, "Notification.EmployeeRequest.CancelledInformativeRequester" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Code", "EntityType", "IsInformative", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { "CancelledManagerInformativeEmployeeRequest", 1L, true, "Cancelled Manager Informative Employee Request", 4L, "Notification.EmployeeRequest.CancelledInformativeManager" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Code", "EntityType", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { "ApprovedRequesterInformativeEmployeeRequest", 1L, "Approved Requester Informative Employee Request", 2L, "Notification.EmployeeRequest.ApprovedInformativeRequester" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Code", "EntityType", "IsInformative", "Name", "NotificationStatusId", "TranslationLabel" },
                values: new object[] { "ApprovedManagerInformativeEmployeeRequest", 1L, true, "Approved Manager Informative Employee Request", 2L, "Notification.EmployeeRequest.ApprovedInformativeManager" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Code", "EntityType", "Name", "TranslationLabel" },
                values: new object[] { "RejectedRequesterInformativeEmployeeRequest", 1L, "Rejected Requester Informative Employee Request", "Notification.EmployeeRequest.RejectedInformativeRequester" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Code", "EntityType", "IsInformative", "Name", "TranslationLabel" },
                values: new object[] { "RejectedManagerInformativeEmployeeRequest", 1L, true, "Rejected Manager Informative Employee Request", "Notification.EmployeeRequest.RejectedInformativeManager" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranslationLabel",
                schema: "tenantnotification",
                table: "NotificationType");

            migrationBuilder.AlterColumn<string>(
                name: "EntityType",
                schema: "tenantnotification",
                table: "NotificationType",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(long),
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "LabelForReceiver",
                schema: "tenantnotification",
                table: "NotificationType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Code", "EntityType", "IsInformative", "LabelForReceiver", "Name" },
                values: new object[] { "PendingFlowEmployeeRequest", "EmployeeRequest", false, "Notification.EmployeeRequest.PendingFlow", "Pending Flow Employee Request" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Code", "EntityType", "IsInformative", "LabelForReceiver", "Name" },
                values: new object[] { "PendingInformativeEmployeeRequest", "EmployeeRequest", true, "Notification.EmployeeRequest.PendingInformativeReceiver", "Pending Informative Employee Request " });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Code", "EntityType", "LabelForReceiver", "Name", "NotificationStatusId" },
                values: new object[] { "ApprovedInformativeEmployeeRequest", "EmployeeRequest", "Notification.EmployeeRequest.ApprovedInformativeReceiver", "Approved Informative Employee Request", 2L });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Code", "EntityType", "IsInformative", "LabelForReceiver", "Name", "NotificationStatusId" },
                values: new object[] { "ApprovedFlowEmployeeRequest", "EmployeeRequest", false, "Notification.EmployeeRequest.PendingFlow", "Approved Flow Employee Request", 2L });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Code", "EntityType", "LabelForReceiver", "Name", "NotificationStatusId" },
                values: new object[] { "CancelApprovedInformativeEmployeeRequest", "EmployeeRequest", "Notification.EmployeeRequest.CancelApprovedInformativeReceiver", "Cancel Approved informative Employee Request ", 4L });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Code", "EntityType", "IsInformative", "LabelForReceiver", "Name", "NotificationStatusId" },
                values: new object[] { "CancelApprovedFlowEmployeeRequest", "EmployeeRequest", false, "Notification.EmployeeRequest.PendingFlow", "Cancel Approved flow Employee Request", 4L });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Code", "EntityType", "LabelForReceiver", "Name" },
                values: new object[] { "RejectedInformativeEmployeeRequest", "EmployeeRequest", "Notification.EmployeeRequest.RejectedInformativeReceiver", "Rejected Informative Employee Request" });

            migrationBuilder.UpdateData(
                schema: "tenantnotification",
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Code", "EntityType", "IsInformative", "LabelForReceiver", "Name" },
                values: new object[] { "RejectedFlowEmployeeRequest", "EmployeeRequest", false, "Notification.EmployeeRequest.PendingFlow", "Rejected Flow Employee Request" });
        }
    }
}
