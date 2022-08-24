﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class CreateActionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentAction",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    ResposibleId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAction_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAction_DocumentId",
                schema: "DocumentManagement",
                table: "DocumentAction",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentAction",
                schema: "DocumentManagement");
        }
    }
}
