using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class AddedDescriptionToRentalReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RentalReservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalReservations_RentalInventoryId",
                table: "RentalReservations",
                column: "RentalInventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalReservations_RentalInventories_RentalInventoryId",
                table: "RentalReservations",
                column: "RentalInventoryId",
                principalTable: "RentalInventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalReservations_RentalInventories_RentalInventoryId",
                table: "RentalReservations");

            migrationBuilder.DropIndex(
                name: "IX_RentalReservations_RentalInventoryId",
                table: "RentalReservations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RentalReservations");
        }
    }
}
