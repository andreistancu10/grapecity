using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AddRecipientIsDepartmentFieldOnDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverDepartmentId",
                schema: "DocumentMangement",
                table: "InternalDocument");

            migrationBuilder.AddColumn<bool>(
                name: "RecipientIsDepartment",
                schema: "DocumentMangement",
                table: "Document",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientIsDepartment",
                schema: "DocumentMangement",
                table: "Document");

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
