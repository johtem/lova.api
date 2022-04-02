using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class WantInfoSMSEmailToPremiseContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WantInfoEmail",
                table: "PremiseContacts",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "WantInfoSMS",
                table: "PremiseContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WantInfoEmail",
                table: "PremiseContacts");

            migrationBuilder.DropColumn(
                name: "WantInfoSMS",
                table: "PremiseContacts");
        }
    }
}
