using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatPilot.Migrations
{
    public partial class AddEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EvenementId",
                table: "Gateaux",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventID",
                table: "Gateaux",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Evenement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenement", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gateaux_EvenementId",
                table: "Gateaux",
                column: "EvenementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gateaux_Evenement_EvenementId",
                table: "Gateaux",
                column: "EvenementId",
                principalTable: "Evenement",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gateaux_Evenement_EvenementId",
                table: "Gateaux");

            migrationBuilder.DropTable(
                name: "Evenement");

            migrationBuilder.DropIndex(
                name: "IX_Gateaux_EvenementId",
                table: "Gateaux");

            migrationBuilder.DropColumn(
                name: "EvenementId",
                table: "Gateaux");

            migrationBuilder.DropColumn(
                name: "EventID",
                table: "Gateaux");
        }
    }
}
