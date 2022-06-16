using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class WorkflowHistory_AddRecipientNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpionionRequestedUntil",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                newName: "OpinionRequestedUntil");

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientName",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.RenameColumn(
                name: "OpinionRequestedUntil",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                newName: "OpionionRequestedUntil");
        }
    }
}
