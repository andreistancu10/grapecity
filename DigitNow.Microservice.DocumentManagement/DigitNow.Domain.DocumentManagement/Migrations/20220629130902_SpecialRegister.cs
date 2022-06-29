using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class SpecialRegister : Migration
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

            migrationBuilder.CreateTable(
                name: "SpecialRegister",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRegister", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_ContactDetailId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRegister_DocumentCategoryId",
                schema: "DocumentMangement",
                table: "SpecialRegister",
                column: "DocumentCategoryId",
                unique: true);

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

            migrationBuilder.DropTable(
                name: "SpecialRegister",
                schema: "DocumentMangement");

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
