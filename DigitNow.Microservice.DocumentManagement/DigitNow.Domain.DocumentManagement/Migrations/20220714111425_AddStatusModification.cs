using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class AddStatusModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StatusModifiedAt",
                schema: "DocumentMangement",
                table: "Document",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "StatusModifiedBy",
                schema: "DocumentMangement",
                table: "Document",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusModifiedAt",
                schema: "DocumentMangement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "StatusModifiedBy",
                schema: "DocumentMangement",
                table: "Document");
        }
    }
}
