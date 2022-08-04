using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Add_OrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderNumber",
                schema: "DocumentMangement",
                table: "SpecialRegisterMappings",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "DocumentMangement",
                table: "SpecialRegisterMappings");
        }
    }
}
