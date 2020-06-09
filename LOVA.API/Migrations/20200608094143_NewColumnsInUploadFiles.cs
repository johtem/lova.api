using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewColumnsInUploadFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDirectories",
                table: "UploadedFiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDirectory",
                table: "UploadedFiles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDirectories",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "IsDirectory",
                table: "UploadedFiles");
        }
    }
}
