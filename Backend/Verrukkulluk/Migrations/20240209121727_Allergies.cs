using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class Allergies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergy_Allergy_AllergyId",
                table: "Allergy");

            migrationBuilder.DropForeignKey(
                name: "FK_Allergy_Recipes_RecipeId",
                table: "Allergy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergy",
                table: "Allergy");

            migrationBuilder.DropIndex(
                name: "IX_Allergy_AllergyId",
                table: "Allergy");

            migrationBuilder.DropColumn(
                name: "AllergyId",
                table: "Allergy");

            migrationBuilder.RenameTable(
                name: "Allergy",
                newName: "Allergies");

            migrationBuilder.RenameIndex(
                name: "IX_Allergy_RecipeId",
                table: "Allergies",
                newName: "IX_Allergies_RecipeId");

            migrationBuilder.AddColumn<int>(
                name: "ImgObjId",
                table: "Allergies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergies",
                table: "Allergies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductAllergies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllergyId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAllergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAllergies_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAllergies_RecipeId",
                table: "ProductAllergies",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergies_Recipes_RecipeId",
                table: "Allergies",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_Recipes_RecipeId",
                table: "Allergies");

            migrationBuilder.DropTable(
                name: "ProductAllergies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergies",
                table: "Allergies");

            migrationBuilder.DropColumn(
                name: "ImgObjId",
                table: "Allergies");

            migrationBuilder.RenameTable(
                name: "Allergies",
                newName: "Allergy");

            migrationBuilder.RenameIndex(
                name: "IX_Allergies_RecipeId",
                table: "Allergy",
                newName: "IX_Allergy_RecipeId");

            migrationBuilder.AddColumn<int>(
                name: "AllergyId",
                table: "Allergy",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergy",
                table: "Allergy",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Allergy_AllergyId",
                table: "Allergy",
                column: "AllergyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergy_Allergy_AllergyId",
                table: "Allergy",
                column: "AllergyId",
                principalTable: "Allergy",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergy_Recipes_RecipeId",
                table: "Allergy",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
