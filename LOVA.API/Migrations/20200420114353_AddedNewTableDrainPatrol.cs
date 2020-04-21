using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class AddedNewTableDrainPatrol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrainPatrols",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slinga = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Tid = table.Column<DateTime>(nullable: false),
                    Aktiv = table.Column<bool>(nullable: false),
                    WellId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainPatrols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrainPatrols_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrainPatrols_WellId",
                table: "DrainPatrols",
                column: "WellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrainPatrols");
        }
    }
}
