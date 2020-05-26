using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class AddedBackgroundColorToRentalInventories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "RentalInventories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "RentalInventories");
        }
    }
}
