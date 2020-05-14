using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewColumnInTableIssueReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Alarm",
                table: "IssueReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLowVacuum",
                table: "IssueReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MasterNode",
                table: "IssueReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alarm",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "IsLowVacuum",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "MasterNode",
                table: "IssueReports");
        }
    }
}
