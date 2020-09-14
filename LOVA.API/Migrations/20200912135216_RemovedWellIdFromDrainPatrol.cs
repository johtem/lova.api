using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class RemovedWellIdFromDrainPatrol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainPatrols_Wells_WellId",
                table: "DrainPatrols");

            migrationBuilder.DropIndex(
                name: "IX_DrainPatrols_WellId",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "WellId",
                table: "DrainPatrols");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WellId",
                table: "DrainPatrols",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DrainPatrols_WellId",
                table: "DrainPatrols",
                column: "WellId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrainPatrols_Wells_WellId",
                table: "DrainPatrols",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
