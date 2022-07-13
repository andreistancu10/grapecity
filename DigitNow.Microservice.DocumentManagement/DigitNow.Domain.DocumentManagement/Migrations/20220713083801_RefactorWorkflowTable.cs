using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class RefactorWorkflowTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                schema: "DocumentMangement",
                table: "WorkflowHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RegistrationNumber",
                schema: "DocumentMangement",
                table: "WorkflowHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
