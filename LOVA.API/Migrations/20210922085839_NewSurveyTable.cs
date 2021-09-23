using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewSurveyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query10 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyCheckBoxes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false),
                    QueryNumber = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyCheckBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyCheckBoxes_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyCheckBoxes_SurveyId",
                table: "SurveyCheckBoxes",
                column: "SurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyCheckBoxes");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
