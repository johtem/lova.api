using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class ChangedtoDoubleInTableActivityPerRow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TimeDiff",
                table: "ActivityPerRows",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TimeDiff",
                table: "ActivityPerRows",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
