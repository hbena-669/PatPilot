using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatPilot.Migrations
{
    public partial class AjoutTypeGateau : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommandeDesc",
                table: "Gateaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GateauxType",
                table: "Gateaux",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageModel",
                table: "Gateaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommandeDesc",
                table: "Gateaux");

            migrationBuilder.DropColumn(
                name: "GateauxType",
                table: "Gateaux");

            migrationBuilder.DropColumn(
                name: "ImageModel",
                table: "Gateaux");
        }
    }
}
