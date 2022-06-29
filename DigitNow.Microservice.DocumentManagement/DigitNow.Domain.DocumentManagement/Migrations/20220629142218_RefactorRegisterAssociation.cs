using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorRegisterAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DocumentSpecialRegisterAssociations_Id",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations");

            migrationBuilder.AddColumn<long>(
                name: "AssociationId",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialRegisterAssociations_Id",
                schema: "DocumentMangement",
                table: "DocumentSpecialRegisterAssociations",
                column: "Id");
        }
    }
}
