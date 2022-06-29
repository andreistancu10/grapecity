using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionType",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.AlterColumn<long>(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "IdentificationNumber",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentTypeDetail",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "ContactDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "ContactDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "ContactDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "ContactDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "DocumentMangement",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "DocumentMangement",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "DocumentMangement",
                table: "ContactDetail");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "DocumentMangement",
                table: "ContactDetail");

            migrationBuilder.AlterColumn<int>(
                name: "RecipientId",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IdentificationNumber",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentTypeDetail",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
