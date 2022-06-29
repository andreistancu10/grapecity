using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class FixOutgoingDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingDocument_ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropColumn(
                name: "ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.AlterColumn<long>(
                name: "ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId",
                principalSchema: "DocumentMangement",
                principalTable: "ContactDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.AlterColumn<int>(
                name: "ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocument_ContactDetail_ContactDetailId1",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId1",
                principalSchema: "DocumentMangement",
                principalTable: "ContactDetail",
                principalColumn: "Id");
        }
    }
}
