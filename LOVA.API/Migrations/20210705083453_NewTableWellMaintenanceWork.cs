using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewTableWellMaintenanceWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WellMaintenanceWorks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WellId = table.Column<long>(type: "bigint", nullable: false),
                    IsPhoto = table.Column<bool>(type: "bit", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellMaintenanceWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellMaintenanceWorks_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WellMaintenanceWorks_WellId",
                table: "WellMaintenanceWorks",
                column: "WellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WellMaintenanceWorks");
        }
    }
}
