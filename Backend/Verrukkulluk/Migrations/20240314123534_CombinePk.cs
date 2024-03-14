using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class CombinePk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_Recipes_RecipeId",
                table: "Allergies");
            
            migrationBuilder.DropIndex(
                name: "IX_Allergies_RecipeId",
                table: "Allergies");
            
            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Allergies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAllergies_Recipes_RecipeId",
                table: "ProductAllergies");
            
            migrationBuilder.DropIndex(
                name: "IX_ProductAllergies_RecipeId",
                table: "ProductAllergies");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "ProductAllergies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAllergies",
                table: "ProductAllergies");
           
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductAllergies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAllergies",
                table: "ProductAllergies",
                columns: new[] { "ProductId", "AllergyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAllergies",
                table: "ProductAllergies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductAllergies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // Make sure the id is unique
            migrationBuilder.Sql("UPDATE ProductAllergies SET Id=ProductId * 100 + AllergyId");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "ProductAllergies",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAllergies",
                table: "ProductAllergies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAllergies_RecipeId",
                table: "ProductAllergies",
                column: "RecipeId");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Allergies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_RecipeId",
                table: "Allergies",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergies_Recipes_RecipeId",
                table: "Allergies",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAllergies_Recipes_RecipeId",
                table: "ProductAllergies",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
