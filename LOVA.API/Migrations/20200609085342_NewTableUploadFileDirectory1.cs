using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class NewTableUploadFileDirectory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadFileDirectories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadFileCategoryId = table.Column<long>(nullable: false),
                    Directory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFileDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadFileDirectories_UploadFileCategories_UploadFileCategoryId",
                        column: x => x.UploadFileCategoryId,
                        principalTable: "UploadFileCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFileDirectories_UploadFileCategoryId",
                table: "UploadFileDirectories",
                column: "UploadFileCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFileDirectories");
        }
    }
}
