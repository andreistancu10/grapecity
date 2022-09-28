using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class ChangeStateToStateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "DocumentManagement",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "DocumentManagement",
                table: "Objective");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "DocumentManagement",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "DocumentManagement",
                table: "Action");

            migrationBuilder.AlterColumn<string>(
                name: "Deadline",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ControlMeasurement",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Risk",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Objective",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Activity",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Action",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Objective");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "DocumentManagement",
                table: "Action");

            migrationBuilder.AlterColumn<string>(
                name: "Deadline",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ControlMeasurement",
                schema: "DocumentManagement",
                table: "RiskControlAction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "DocumentManagement",
                table: "Risk",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "DocumentManagement",
                table: "Objective",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "DocumentManagement",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "DocumentManagement",
                table: "Action",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
