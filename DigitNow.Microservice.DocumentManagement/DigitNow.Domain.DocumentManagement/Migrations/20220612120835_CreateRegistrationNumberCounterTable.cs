using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class CreateRegistrationNumberCounterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationNumberCounter",
                schema: "documentamangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationNumberCounter", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationNumberCounter",
                schema: "documentamangement");
        }
    }
}
