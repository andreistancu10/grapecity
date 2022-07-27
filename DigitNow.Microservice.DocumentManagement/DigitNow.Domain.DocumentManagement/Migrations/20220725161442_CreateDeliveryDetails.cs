using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class CreateDeliveryDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryDetail",
                schema: "DocumentMangement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryMode = table.Column<int>(type: "int", nullable: false),
                    DirectShipping = table.Column<int>(type: "int", nullable: false),
                    Post = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocument_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "DeliveryDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocument_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                column: "DeliveryDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingDocument_DeliveryDetail_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument",
                column: "DeliveryDetailsId",
                principalSchema: "DocumentMangement",
                principalTable: "DeliveryDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocument_DeliveryDetail_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument",
                column: "DeliveryDetailsId",
                principalSchema: "DocumentMangement",
                principalTable: "DeliveryDetail",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingDocument_DeliveryDetail_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocument_DeliveryDetail_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropTable(
                name: "DeliveryDetail",
                schema: "DocumentMangement");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingDocument_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropIndex(
                name: "IX_IncomingDocument_DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument");

            migrationBuilder.DropColumn(
                name: "DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "OutgoingDocument");

            migrationBuilder.DropColumn(
                name: "DeliveryDetailsId",
                schema: "DocumentMangement",
                table: "IncomingDocument");
        }
    }
}
