using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ShiftIn.Domain.TenantNotification.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tenantnotification");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "tenantnotification",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(maxLength: 2048, nullable: false),
                    NotificationTypeId = table.Column<long>(nullable: false),
                    NotificationStatusId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    EntityId = table.Column<long>(nullable: true),
                    EntityTypeId = table.Column<long>(nullable: true),
                    Seen = table.Column<bool>(nullable: false),
                    SeenOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "tenantnotification");
        }
    }
}
