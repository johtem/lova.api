using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class AddedIsScriptionToMailSubScription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailTypes_MailTypes_MailTypeId",
                table: "MailTypes");

            migrationBuilder.DropIndex(
                name: "IX_MailTypes_MailTypeId",
                table: "MailTypes");

            migrationBuilder.DropColumn(
                name: "MailTypeId",
                table: "MailTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsScription",
                table: "MailSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScription",
                table: "MailSubscriptions");

            migrationBuilder.AddColumn<long>(
                name: "MailTypeId",
                table: "MailTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailTypes_MailTypeId",
                table: "MailTypes",
                column: "MailTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MailTypes_MailTypes_MailTypeId",
                table: "MailTypes",
                column: "MailTypeId",
                principalTable: "MailTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
