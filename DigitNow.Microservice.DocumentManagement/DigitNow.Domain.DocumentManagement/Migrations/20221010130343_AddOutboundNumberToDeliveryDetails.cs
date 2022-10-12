using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AddOutboundNumberToDeliveryDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutboundNumber",
                schema: "DocumentManagement",
                table: "DeliveryDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutboundNumber",
                schema: "DocumentManagement",
                table: "DeliveryDetail");
        }
    }
}
