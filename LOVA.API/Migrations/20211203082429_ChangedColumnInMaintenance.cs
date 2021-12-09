using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class ChangedColumnInMaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LatestMaintenances_Maintenances_MaintenanceId",
                table: "LatestMaintenances");

            migrationBuilder.AlterColumn<int>(
                name: "RecurringFrequence",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaintenanceId",
                table: "LatestMaintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LatestMaintenances_Maintenances_MaintenanceId",
                table: "LatestMaintenances",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LatestMaintenances_Maintenances_MaintenanceId",
                table: "LatestMaintenances");

            migrationBuilder.AlterColumn<string>(
                name: "RecurringFrequence",
                table: "Maintenances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaintenanceId",
                table: "LatestMaintenances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LatestMaintenances_Maintenances_MaintenanceId",
                table: "LatestMaintenances",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
