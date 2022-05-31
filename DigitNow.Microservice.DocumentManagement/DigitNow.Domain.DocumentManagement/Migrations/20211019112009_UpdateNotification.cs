using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class UpdateNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FromUserId",
                schema: "tenantnotification",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserId",
                schema: "tenantnotification",
                table: "Notification");
        }
    }
}
