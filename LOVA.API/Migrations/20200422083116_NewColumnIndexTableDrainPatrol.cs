using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewColumnIndexTableDrainPatrol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktiv",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "Slinga",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "Tid",
                table: "DrainPatrols");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DrainPatrols",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "DrainPatrols",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Master_node",
                table: "DrainPatrols",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "DrainPatrols",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "Master_node",
                table: "DrainPatrols");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "DrainPatrols");

            migrationBuilder.AddColumn<bool>(
                name: "Aktiv",
                table: "DrainPatrols",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slinga",
                table: "DrainPatrols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tid",
                table: "DrainPatrols",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
