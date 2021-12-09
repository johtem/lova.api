using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewMaintenanceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceGroup = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurringFrequence = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LatestMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastMaintenance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaintenanceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatestMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LatestMaintenances_Maintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LatestMaintenances_MaintenanceId",
                table: "LatestMaintenances",
                column: "MaintenanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LatestMaintenances");

            migrationBuilder.DropTable(
                name: "Maintenances");
        }
    }
}
