using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatPilot.Migrations
{
    public partial class AddingGateauTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gateau_Enseigne_EnseigneId",
                table: "Gateau");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Gateau_GateauId",
                table: "Ingredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gateau",
                table: "Gateau");

            migrationBuilder.RenameTable(
                name: "Gateau",
                newName: "Gateaux");

            migrationBuilder.RenameIndex(
                name: "IX_Gateau_EnseigneId",
                table: "Gateaux",
                newName: "IX_Gateaux_EnseigneId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Gateaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gateaux",
                table: "Gateaux",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gateaux_Enseigne_EnseigneId",
                table: "Gateaux",
                column: "EnseigneId",
                principalTable: "Enseigne",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Gateaux_GateauId",
                table: "Ingredient",
                column: "GateauId",
                principalTable: "Gateaux",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gateaux_Enseigne_EnseigneId",
                table: "Gateaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Gateaux_GateauId",
                table: "Ingredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gateaux",
                table: "Gateaux");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Gateaux");

            migrationBuilder.RenameTable(
                name: "Gateaux",
                newName: "Gateau");

            migrationBuilder.RenameIndex(
                name: "IX_Gateaux_EnseigneId",
                table: "Gateau",
                newName: "IX_Gateau_EnseigneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gateau",
                table: "Gateau",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gateau_Enseigne_EnseigneId",
                table: "Gateau",
                column: "EnseigneId",
                principalTable: "Enseigne",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Gateau_GateauId",
                table: "Ingredient",
                column: "GateauId",
                principalTable: "Gateau",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
