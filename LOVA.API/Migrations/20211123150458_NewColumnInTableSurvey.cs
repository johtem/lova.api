﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewColumnInTableSurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SurveyName",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurveyName",
                table: "Surveys");
        }
    }
}
