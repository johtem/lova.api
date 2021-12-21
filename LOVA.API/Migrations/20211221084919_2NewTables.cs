using Microsoft.EntityFrameworkCore.Migrations;

namespace LOVA.API.Migrations
{
    public partial class _2NewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Association",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "MaintenanceGroup",
                table: "Maintenances");

            migrationBuilder.AddColumn<int>(
                name: "AssociationId",
                table: "Maintenances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceGroupId",
                table: "Maintenances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Associations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VAT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssociationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceGroups_Associations_AssociationId",
                        column: x => x.AssociationId,
                        principalTable: "Associations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_AssociationId",
                table: "Maintenances",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_MaintenanceGroupId",
                table: "Maintenances",
                column: "MaintenanceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceGroups_AssociationId",
                table: "MaintenanceGroups",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Associations_AssociationId",
                table: "Maintenances",
                column: "AssociationId",
                principalTable: "Associations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceGroups_MaintenanceGroupId",
                table: "Maintenances",
                column: "MaintenanceGroupId",
                principalTable: "MaintenanceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Associations_AssociationId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceGroups_MaintenanceGroupId",
                table: "Maintenances");

            migrationBuilder.DropTable(
                name: "MaintenanceGroups");

            migrationBuilder.DropTable(
                name: "Associations");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_AssociationId",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_MaintenanceGroupId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "MaintenanceGroupId",
                table: "Maintenances");

            migrationBuilder.AddColumn<int>(
                name: "Association",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceGroup",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
