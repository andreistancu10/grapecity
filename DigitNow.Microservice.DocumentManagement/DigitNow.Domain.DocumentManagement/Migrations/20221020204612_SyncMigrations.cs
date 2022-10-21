using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class SyncMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateRegistration = table.Column<long>(type: "bigint", nullable: false),
                    CommercialRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatPayer = table.Column<bool>(type: "bit", nullable: false),
                    CompanyType = table.Column<int>(type: "int", nullable: false),
                    RegisteredWorkplace = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredOfficeContactDetailId = table.Column<long>(type: "bigint", nullable: true),
                    RegisteredWorkplaceContactDetailId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_ContactDetail_RegisteredOfficeContactDetailId",
                        column: x => x.RegisteredOfficeContactDetailId,
                        principalSchema: "DocumentManagement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Supplier_ContactDetail_RegisteredWorkplaceContactDetailId",
                        column: x => x.RegisteredWorkplaceContactDetailId,
                        principalSchema: "DocumentManagement",
                        principalTable: "ContactDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierLegalRepresentative",
                schema: "DocumentManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepresentativeQuality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierLegalRepresentative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierLegalRepresentative_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "DocumentManagement",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_RegisteredOfficeContactDetailId",
                schema: "DocumentManagement",
                table: "Supplier",
                column: "RegisteredOfficeContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_RegisteredWorkplaceContactDetailId",
                schema: "DocumentManagement",
                table: "Supplier",
                column: "RegisteredWorkplaceContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLegalRepresentative_SupplierId",
                schema: "DocumentManagement",
                table: "SupplierLegalRepresentative",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierLegalRepresentative",
                schema: "DocumentManagement");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "DocumentManagement");
        }
    }
}
