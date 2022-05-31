using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class NotificationTypeAndStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationStatus",
                schema: "tenantnotification",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Code = table.Column<string>(maxLength: 256, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationType",
                schema: "tenantnotification",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Code = table.Column<string>(maxLength: 256, nullable: false),
                    IsInformative = table.Column<bool>(nullable: false),
                    IsUrgent = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    NotificationStatusId = table.Column<long>(nullable: false),
                    EntityType = table.Column<string>(maxLength: 256, nullable: false),
                    LabelForSender = table.Column<string>(nullable: true),
                    LabelForReceiver = table.Column<string>(nullable: true),
                    Expression = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationType_NotificationStatus_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalSchema: "tenantnotification",
                        principalTable: "NotificationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationStatus",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1L, true, "Pending", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pending" },
                    { 2L, true, "Approved", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Approved" },
                    { 3L, true, "Rejected", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Rejected" },
                    { 4L, true, "CancelApproved", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "CancelApproved" }
                });

            migrationBuilder.InsertData(
                schema: "tenantnotification",
                table: "NotificationType",
                columns: new[] { "Id", "Active", "Code", "CreatedBy", "CreatedOn", "EntityType", "Expression", "IsInformative", "IsUrgent", "LabelForReceiver", "LabelForSender", "ModifiedBy", "ModifiedOn", "Name", "NotificationStatusId" },
                values: new object[,]
                {
                    { 1L, true, "PendingFlowEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, false, false, "Notification.EmployeeRequest.PendingFlow", "Notification.EmployeeRequest.PendingFlowSender", null, null, "Pending Flow Employee Request", 1L },
                    { 15L, true, "CanceledBySourceInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.CanceledBySourceInformativeReceiver", "Notification.CoverGapRequest.CanceledBySourceInformativeSender", null, null, "Canceled By Source Informative CoverGap Request ", 4L },
                    { 14L, true, "CanceledBySourceFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.CanceledBySourceFlow", null, null, null, "Canceled By Source Flow CoverGap Request ", 4L },
                    { 6L, true, "CancelApprovedflowEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, false, false, "Notification.EmployeeRequest.PendingFlow", null, null, null, "Cancel Approved flow Employee Request", 4L },
                    { 5L, true, "CancelApprovedinformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, true, false, "Notification.EmployeeRequest.CancelApprovedInformativeReceiver", "Notification.EmployeeRequest.CancelApprovedInformativeSender", null, null, "Cancel Approved informative Employee Request ", 4L },
                    { 23L, true, "RejectedByExpiredTimeInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.RejectedByExpiredTimeInformativeReceiver", "Notification.CoverGapRequest.RejectedByExpiredTimeInformativeSender", null, null, "Rejected By Expired Time Informative CoverGap Request", 3L },
                    { 22L, true, "RejectedByExpiredTimeFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.RejectedByExpiredTimeFlow", null, null, null, "Rejected By Expired Time Flow CoverGap Request", 3L },
                    { 21L, true, "RejectedByIneligibilityInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.RejectedByIneligibilityInformativeReceiver", "Notification.CoverGapRequest.RejectedByIneligibilityInformativeSender", null, null, "Rejected By Ineligibility Informative CoverGap Request", 3L },
                    { 20L, true, "RejectedByIneligibilityFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.RejectedByIneligibilityFlow", null, null, null, "Rejected By Ineligibility Flow CoverGap Request ", 3L },
                    { 19L, true, "RejectApprovedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.RejectApprovedInformativeReceiver", "Notification.CoverGapRequest.RejectApprovedInformativeSender", null, null, "Reject Approved Informative CoverGap Request ", 3L },
                    { 18L, true, "RejectApprovedFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.RejectApprovedFlow", null, null, null, "Reject Approved Flow CoverGap Request", 3L },
                    { 8L, true, "RejectedFlowEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, false, false, "Notification.EmployeeRequest.PendingFlow", null, null, null, "Rejected Flow Employee Request", 3L },
                    { 7L, true, "RejectedInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, true, false, "Notification.EmployeeRequest.RejectedInformativeReceiver", "Notification.EmployeeRequest.RejectedInformativeSender", null, null, "Rejected Informative Employee Request", 3L },
                    { 13L, true, "ApprovedInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.ApprovedInformativeReceiver", "Notification.CoverGapRequest.ApprovedInformativeSender", null, null, "Approved Informative CoverGap Request", 2L },
                    { 12L, true, "ApprovedFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.ApprovedFlow", "Notification.CoverGapRequest.ApprovedFlowSender", null, null, "Approved Flow CoverGap Request", 2L },
                    { 4L, true, "ApprovedFlowEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, false, false, "Notification.EmployeeRequest.PendingFlow", null, null, null, "Approved Flow Employee Request", 2L },
                    { 3L, true, "ApprovedInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, true, false, "Notification.EmployeeRequest.ApprovedInformativeReceiver", "Notification.EmployeeRequest.ApprovedInformativeSender", null, null, "Approved Informative Employee Request", 2L },
                    { 24L, true, "PendingInformativeBusinessObjectives", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BusinessObjectives", null, true, false, "Notification.BusinessObjectives.PendingInformativeForReceiver", "Notification.BusinessObjectives.PendingInformativeForSender", null, null, "Pending Informative Business Objectives", 1L },
                    { 11L, true, "PendingInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.PendingInformativeReceiver", "Notification.CoverGapRequest.PendingInformativeSender", null, null, "Pending Informative CoverGap Request ", 1L },
                    { 10L, true, "PendingFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.PendingFlow", "Notification.CoverGapRequest.PendingFlowSender", null, null, "Pending Flow CoverGap Request", 1L },
                    { 9L, true, "PendingInformativeLocationProgram", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LocationProgram", null, true, false, "Notification.LocationProgram.PendingInformativeForReceiver", "Notification.LocationProgram.PendingInformativeForSender", null, null, "Pending Informative Location Program ", 1L },
                    { 2L, true, "PendingInformativeEmployeeRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeRequest", null, true, false, "Notification.EmployeeRequest.PendingInformativeReceiver", "Notification.EmployeeRequest.PendingInformativeSender", null, null, "Pending Informative Employee Request ", 1L },
                    { 16L, true, "CanceledByTargetFlowCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, false, false, "Notification.CoverGapRequest.CanceledByTargetFlow", null, null, null, "Canceled By Target Flow CoverGap Request ", 4L },
                    { 17L, true, "CanceledByTargetInformativeCoverGapRequest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoverGapRequest", null, true, false, "Notification.CoverGapRequest.CanceledByTargetInformativeReceiver", "Notification.CoverGapRequest.CanceledByTargetInformativeSender", null, null, "Canceled By Target Informative CoverGap Request ", 4L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatus_Code",
                schema: "tenantnotification",
                table: "NotificationStatus",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_Code",
                schema: "tenantnotification",
                table: "NotificationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_NotificationStatusId",
                schema: "tenantnotification",
                table: "NotificationType",
                column: "NotificationStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationType",
                schema: "tenantnotification");

            migrationBuilder.DropTable(
                name: "NotificationStatus",
                schema: "tenantnotification");
        }
    }
}
