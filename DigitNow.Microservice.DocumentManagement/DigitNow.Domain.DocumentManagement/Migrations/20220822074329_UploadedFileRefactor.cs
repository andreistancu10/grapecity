using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class UploadedFileRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentCategoryId",
                schema: "DocumentMangement",
                table: "UploadedFile");

            migrationBuilder.AddColumn<string>(
                name: "UsageContext",
                schema: "DocumentMangement",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageLocation",
                schema: "DocumentMangement",
                table: "UploadedFile",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageContext",
                schema: "DocumentMangement",
                table: "UploadedFile");

            migrationBuilder.DropColumn(
                name: "UsageLocation",
                schema: "DocumentMangement",
                table: "UploadedFile");

            migrationBuilder.AddColumn<long>(
                name: "DocumentCategoryId",
                schema: "DocumentMangement",
                table: "UploadedFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
