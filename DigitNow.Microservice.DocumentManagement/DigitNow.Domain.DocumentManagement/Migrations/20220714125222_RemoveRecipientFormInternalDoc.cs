using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RemoveRecipientFormInternalDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverDepartmentId",
                schema: "DocumentMangement",
                table: "InternalDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiverDepartmentId",
                schema: "DocumentMangement",
                table: "InternalDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
